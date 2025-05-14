using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Library.Data;
using E_Library.Model;
using Microsoft.AspNetCore.Identity;
using E_Library.Services;
using E_Library.Services.Interface;
using E_Library.Entities;


namespace E_Library.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IEmailService _emailService;

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
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
            if (totalBooks >= 5)
                discount += 0.05m;

            if (int.TryParse(user.TotalOrder, out int totalOrders) && totalOrders >= 10)
                discount += 0.10m;

            decimal discountAmount = subtotal * discount;
            decimal totalAfterDiscount = subtotal - discountAmount;

            string claimCode = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();

            var order = new Order
            {
                User_Id = user.Id,
                Status = "Placed",
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAfterDiscount,
                DiscountAmount = discountAmount,
                ClaimCode = claimCode, // ✅ ADD THIS LINE
                OrderItems = cartItems.Select(c =>
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
                }).ToList()
            };


            _context.Orders.Add(order);
            _context.Carts.RemoveRange(cartItems);
            user.TotalOrder = (totalOrders + 1).ToString();
            await _context.SaveChangesAsync();

            // ✅ Send Email with Claim Code

            var bill = $@"
            Hello {user.UserName},

            📦 Thank you for your order!

            🧾 Order #: {order.Order_Id}
            📅 Date: {order.OrderDate:yyyy-MM-dd HH:mm}
            💵 Total: {order.TotalAmount:C}
            🎁 Discount: {order.DiscountAmount:C}
            🔑 Claim Code: {claimCode}

            Books Purchased:
            {string.Join("\n", order.OrderItems.Select(i => $"- {i.Book.Title} x{i.Quantity}"))}

            Please keep this email as your receipt and proof of claim.

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
        public async Task<IActionResult> Cancel(Guid orderId)
        {
            var user = await _userManager.GetUserAsync(User);
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Order_Id == orderId && o.User_Id == user.Id);

            if (order != null && order.Status == "Pending")
            {
                order.Status = "Cancelled";
                await _context.SaveChangesAsync();
                TempData["Message"] = "🛑 Order cancelled.";
            }

            return RedirectToAction("MyOrders");
        }
    }
}
