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
    }
}
