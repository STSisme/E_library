// WishlistController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Library.Data;
using E_Library.Model;
using Microsoft.AspNetCore.Identity;

namespace E_Library.Controllers
{


    [Authorize]
    public class WishlistController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WishlistController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Guid bookId)
        {
            Console.WriteLine("\n📥 Wishlist Add POST hit");
            Console.WriteLine("📚 Book ID: " + bookId);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                Console.WriteLine("❌ User not found");
                return Unauthorized();
            }

            Console.WriteLine("👤 User ID: " + user.Id);

            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                Console.WriteLine("❌ Book not found in DB: " + bookId);
                TempData["Alert"] = "Error: Book not found.";
                return RedirectToAction("Details", "Books", new { id = bookId });
            }

            var alreadyExists = await _context.Wishlists
                .AnyAsync(w => w.User_Id == user.Id && w.Book_Id == bookId);

            Console.WriteLine("🔁 alreadyExists = " + alreadyExists);

            if (alreadyExists)
            {
                TempData["Alert"] = "This book is already in your wishlist.";
                return RedirectToAction("Details", "Books", new { id = bookId });
            }

            var wishlist = new Wishlist
            {
                Wishlist_Id = Guid.NewGuid(),
                User_Id = user.Id,
                Book_Id = bookId,
                AddedAt = DateTime.UtcNow
            };

            _context.Wishlists.Add(wishlist);
            try
            {
                int result = await _context.SaveChangesAsync();
                Console.WriteLine("✅ Rows affected: " + result);
                TempData["Alert"] = "Book successfully added to your wishlist.";
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ DB SAVE ERROR: " + ex.Message);
                TempData["Alert"] = "Failed to add book to wishlist.";
            }

            return RedirectToAction("Details", "Books", new { id = bookId });
        }

        public async Task<IActionResult> MyWishlist()
        {
            var user = await _userManager.GetUserAsync(User);
            var items = await _context.Wishlists
                .Include(w => w.Book)
                .Where(w => w.User_Id == user.Id)
                .ToListAsync();

            return View(items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(Guid bookId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var entry = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.User_Id == user.Id && w.Book_Id == bookId);

            if (entry != null)
            {
                _context.Wishlists.Remove(entry);
                await _context.SaveChangesAsync();
                TempData["Message"] = "✅ Removed from wishlist.";
            }

            return RedirectToAction("Details", "Books", new { id = bookId });
        }

    }
}
