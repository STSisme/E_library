using E_Library.Data;
using E_Library.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class WishlistController : Controller
{
    private readonly ApplicationDbContext _context;

    public WishlistController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Add to wishlist
    [HttpPost]
    public async Task<IActionResult> Add(Guid bookId)
    {
        if (!User.Identity.IsAuthenticated || User.FindFirst("UserId") == null)
        {
            TempData["AlertMessage"] = "You must be logged in to add books to your wishlist.";
            return RedirectToAction("Login", "User");
        }

        // Assuming "UserId" is a string (for IdentityUser)
        var userId = User.FindFirst("UserId")?.Value;

        if (userId == null)
        {
            TempData["AlertMessage"] = "User ID is not available.";
            return RedirectToAction("Login", "User");
        }

        var exists = await _context.Wishlists
            .AnyAsync(w => w.User_Id == userId && w.Book_Id == bookId);

        if (!exists)
        {
            var wishlist = new Wishlist
            {
                User_Id = userId,  // Assuming "User_Id" is stored as a string
                Book_Id = bookId
            };
            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index", "Books");
    }

    // View wishlist
    public async Task<IActionResult> Index()
    {
        if (!User.Identity.IsAuthenticated || User.FindFirst("UserId") == null)
        {
            TempData["AlertMessage"] = "Please login to view your wishlist.";
            return RedirectToAction("Login", "User");
        }

        var userId = User.FindFirst("UserId")?.Value;

        if (userId == null)
        {
            TempData["AlertMessage"] = "User ID is not available.";
            return RedirectToAction("Login", "User");
        }

        var wishlistItems = await _context.Wishlists
            .Include(w => w.Book)
            .Where(w => w.User_Id == userId)
            .ToListAsync();

        return View(wishlistItems);
    }


    // Remove from wishlist
    public async Task<IActionResult> Remove(Guid wishlistId)
    {
        if (!User.Identity.IsAuthenticated || User.FindFirst("UserId") == null)
        {
            TempData["AlertMessage"] = "You must be logged in to remove books from your wishlist.";
            return RedirectToAction("Login", "User");
        }

        var item = await _context.Wishlists.FindAsync(wishlistId);
        if (item != null)
        {
            _context.Wishlists.Remove(item);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }
}
