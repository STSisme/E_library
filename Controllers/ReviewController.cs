using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using E_Library.Data;
using E_Library.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Library.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Guid bookId, int rating, string comment)
        {
            var user = await _userManager.GetUserAsync(User);

            // ✅ Ensure purchased
            var hasPurchased = await _context.Orders
                .Where(o => o.User_Id == user.Id && o.Status != "Cancelled")
                .SelectMany(o => o.OrderItems)
                .AnyAsync(oi => oi.Book_Id == bookId);

            if (!hasPurchased)
            {
                TempData["Message"] = "❌ You can only review books you have purchased.";
                return RedirectToAction("Details", "Books", new { id = bookId });
            }

            // ✅ Ensure not already reviewed
            var alreadyReviewed = await _context.Reviews
                .AnyAsync(r => r.User_Id == user.Id && r.Book_Id == bookId);

            if (alreadyReviewed)
            {
                TempData["Message"] = "⚠️ You have already reviewed this book.";
                return RedirectToAction("Details", "Books", new { id = bookId });
            }

            var review = new Review
            {
                User_Id = user.Id,
                Book_Id = bookId,
                Rating = rating,
                Comment = comment
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            TempData["Message"] = "✅ Thank you for your review!";
            return RedirectToAction("Details", "Books", new { id = bookId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid reviewId)
        {
            var user = await _userManager.GetUserAsync(User);
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Review_Id == reviewId && r.User_Id == user.Id);

            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
                TempData["Message"] = "🗑️ Your review was deleted.";
            }

            return RedirectToAction("Details", "Books", new { id = review.Book_Id });
        }

    }
}
