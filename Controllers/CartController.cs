using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Library.Data;
using E_Library.Model;
using Microsoft.AspNetCore.Identity;

namespace E_Library.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Guid bookId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Message"] = "❌ User not found!";
                return RedirectToAction("Details", "Books", new { id = bookId });
            }

            var existing = await _context.Carts
                .FirstOrDefaultAsync(c => c.Book_Id == bookId && c.User_Id == user.Id);

            if (existing != null)
            {
                existing.Quantity++;
                TempData["Message"] = "🔁 Quantity updated in cart.";
            }
            else
            {
                _context.Carts.Add(new Cart
                {
                    User_Id = user.Id,
                    Book_Id = bookId,
                    Quantity = 1
                });
                TempData["Message"] = "✅ Book added to cart.";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Books", new { id = bookId });
        }

        public async Task<IActionResult> MyCart()
        {
            var user = await _userManager.GetUserAsync(User);
            var items = await _context.Carts
                .Include(c => c.Book)
                .Where(c => c.User_Id == user.Id)
                .ToListAsync();

            return View(items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(Guid bookId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var entry = await _context.Carts
                .FirstOrDefaultAsync(c => c.User_Id == user.Id && c.Book_Id == bookId);

            if (entry != null)
            {
                _context.Carts.Remove(entry);
                await _context.SaveChangesAsync();
                TempData["Message"] = "🗑️ Removed from cart.";
            }

            return RedirectToAction("Details", "Books", new { id = bookId });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Increment(Guid bookId)
        {
            var user = await _userManager.GetUserAsync(User);
            var item = await _context.Carts.FirstOrDefaultAsync(c => c.User_Id == user.Id && c.Book_Id == bookId);

            if (item != null)
            {
                item.Quantity++;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("MyCart");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Decrement(Guid bookId)
        {
            var user = await _userManager.GetUserAsync(User);
            var item = await _context.Carts.FirstOrDefaultAsync(c => c.User_Id == user.Id && c.Book_Id == bookId);

            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _context.Carts.Remove(item);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("MyCart");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var cartItems = await _context.Carts
                .Include(c => c.Book)
                .Where(c => c.User_Id == user.Id)
                .ToListAsync();

            if (!cartItems.Any())
            {
                TempData["Message"] = "🛒 Your cart is empty.";
                return RedirectToAction("MyCart");
            }

            int totalBooks = cartItems.Sum(c => c.Quantity);
            decimal subtotal = cartItems.Sum(c => c.Book.Price * c.Quantity);

            decimal discount = 0;

            // ✅ 5% discount for 5+ books
            if (totalBooks >= 5)
                discount += 0.05m;

            // ✅ Additional 10% discount for 10+ successful orders
            if (int.TryParse(user.TotalOrder, out int successfulOrders) && successfulOrders >= 10)
                discount += 0.10m;

            // ✅ Final discount calculation
            decimal discountAmount = subtotal * discount;
            decimal totalAfterDiscount = subtotal - discountAmount;

            var order = new Order
            {
                User_Id = user.Id,
                OrderDate = DateTime.UtcNow,
                Status = "Placed",
                TotalAmount = totalAfterDiscount,
                DiscountAmount = discountAmount,
                OrderItems = cartItems.Select(c => new OrderItem
                {
                    Book_Id = c.Book_Id,
                    Quantity = c.Quantity,
                    UnitPrice = c.Book.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.Carts.RemoveRange(cartItems);

            // ✅ Update user's TotalOrder count
            user.TotalOrder = (successfulOrders + 1).ToString();

            await _context.SaveChangesAsync();

            TempData["Message"] = $"✅ Order placed! You saved {discountAmount:C}.";
            return RedirectToAction("MyOrders", "Order");
        }


    }
}
