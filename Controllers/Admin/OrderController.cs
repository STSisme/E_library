using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Library.Data;
using E_Library.Model;
using Microsoft.AspNetCore.Identity;

namespace E_Library.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Place()
        {
            var user = await _userManager.GetUserAsync(User);
            var cartItems = await _context.Carts
                .Include(c => c.Book)
                .Where(c => c.User_Id == user.Id)
                .ToListAsync();

            if (!cartItems.Any())
            {
                TempData["Message"] = "❌ Your cart is empty.";
                return RedirectToAction("MyCart", "Cart");
            }

            var order = new Order
            {
                User_Id = user.Id,
                Status = "Pending",
                OrderItems = new List<OrderItem>(),
                TotalAmount = cartItems.Sum(c => c.Book.Price * c.Quantity)
            };

            foreach (var item in cartItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    Book_Id = item.Book_Id,
                    Quantity = item.Quantity,
                    UnitPrice = item.Book.Price
                });
            }

            _context.Orders.Add(order);
            _context.Carts.RemoveRange(cartItems); // Clear cart
            await _context.SaveChangesAsync();

            TempData["Message"] = "✅ Order placed successfully!";
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
