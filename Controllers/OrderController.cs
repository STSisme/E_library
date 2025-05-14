using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Library.Data;
using E_Library.Model;
using Microsoft.AspNetCore.Identity;
using E_Library.Services;
using E_Library.Services.Interface;
using E_Library.Entities;
using Microsoft.AspNetCore.SignalR;
using E_Library.Hubs;
using static System.Reflection.Metadata.BlobBuilder;


namespace E_Library.Controllers
{


    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly IEmailService _emailService;
        private readonly IOrderService _orderService;

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailService emailService, IOrderService orderService, IHubContext<NotificationHub> hub)
        {
            _context = context;
            _userManager = userManager;
            _orderService = orderService;
            _emailService = emailService;
            _hub = hub;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Place()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var cartItems = await _context.Carts
                .Include(c => c.Book)
                .Where(c => c.User_Id == user.Id)
                .ToListAsync();

            if (!cartItems.Any())
            {
                TempData["Message"] = "❌ Your cart is empty.";
                return RedirectToAction("MyCart", "Cart");
            }

            int totalBooks = cartItems.Sum(c => c.Quantity);
            decimal subtotal = cartItems.Sum(c =>
            {
                decimal price = c.Book.Price;
                if (c.Book.IsOnSale &&
                    c.Book.SaleStartDate <= DateTime.UtcNow &&
                    c.Book.SaleEndDate >= DateTime.UtcNow &&
                    c.Book.DiscountPrice.HasValue)
                {
                    price = c.Book.DiscountPrice.Value;
                }
                return price * c.Quantity;
            });

            decimal discount = 0;
            if (totalBooks >= 5) discount += 0.05m;
            if (int.TryParse(user.TotalOrder, out int totalOrders) && totalOrders >= 10)
                discount += 0.10m;

            decimal discountAmount = subtotal * discount;
            decimal totalAfterDiscount = subtotal - discountAmount;

            string claimCode = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();

            var orderItems = cartItems.Select(c =>
            {
                decimal price = c.Book.Price;
                if (c.Book.IsOnSale &&
                    c.Book.SaleStartDate <= DateTime.UtcNow &&
                    c.Book.SaleEndDate >= DateTime.UtcNow &&
                    c.Book.DiscountPrice.HasValue)
                {
                    price = c.Book.DiscountPrice.Value;
                }

                return new OrderItem
                {
                    Book_Id = c.Book_Id,
                    Quantity = c.Quantity,
                    UnitPrice = price
                };
            }).ToList();

            var order = new Order
            {
                User_Id = user.Id,
                Status = "Placed",
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAfterDiscount,
                DiscountAmount = discountAmount,
                ClaimCode = claimCode,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);
            _context.Carts.RemoveRange(cartItems);
            user.TotalOrder = (totalOrders + 1).ToString();
            await _context.SaveChangesAsync();

            // ✅ Real-time Broadcast
            string books = string.Join(", ", orderItems.Select(i => i.Book?.Title ?? "[Unknown]"));
            await _hub.Clients.All.SendAsync("ReceiveNotification", $"📣 {user.UserName} just purchased: {books}");

            // ✅ Email
            var bill = $@"
            Hello {user.UserName},

            📦 Thank you for your order!

            🧾 Order #: {order.Order_Id}
            📅 Date: {order.OrderDate:yyyy-MM-dd HH:mm}
            💵 Total: {order.TotalAmount:C}
            🎁 Discount: {order.DiscountAmount:C}
            🔑 Claim Code: {claimCode}

            Books Purchased:
            {string.Join("\n", order.OrderItems.Select(i => $"- x{i.Quantity}"))}

            Regards,
            E-Library Team
            ";

            await _emailService.SendEmailAsync(user.Email, "📧 Your E-Library Order Receipt", bill);

            TempData["Message"] = $"✅ Order placed! Receipt sent to {user.Email}. You saved {discountAmount:C}.";
            return RedirectToAction("MyOrders");
        }


        public async Task<IActionResult> MyOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                .Where(o => o.User_Id == user.Id)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(Guid orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                TempData["Message"] = "Order not found.";
                return RedirectToAction("MyOrders");
            }

            // Only allow cancelling orders with specific statuses
            if (order.Status == "Placed" || order.Status == "Pending")
            {
                order.Status = "Cancelled";  // Update the order status
                _orderService.UpdateOrder(order);  // Save the changes to the database
                TempData["Message"] = "Your order has been cancelled successfully.";
            }
            else
            {
                TempData["Message"] = "You cannot cancel this order at this stage.";
            }

            return RedirectToAction("MyOrders");
        }

    }
}
