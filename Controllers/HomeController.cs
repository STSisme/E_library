using E_Library.Data;
using E_Library.Model;
using E_Library.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Library.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get genres
            var genres = await _context.Books
                .Select(b => b.Genre)
                .Distinct()
                .ToListAsync();

            // Categorized books by sections like Bestsellers, Award Winners, etc.
            var categorizedBooks = new Dictionary<string, List<Book>>
            {
                { "Bestsellers", await _context.Books.Where(b => b.IsBestseller).Take(10).ToListAsync() },
                { "Award Winners", await _context.Books.Where(b => b.IsAwardWinner).Take(10).ToListAsync() },
                { "New Releases", await _context.Books.OrderByDescending(b => b.PublishedDate).Take(10).ToListAsync() },
                { "Coming Soon", await _context.Books.Where(b => b.IsComingSoon).Take(10).ToListAsync() },
                { "Deals", await _context.Books.Where(b => b.IsOnSale).Take(10).ToListAsync() }
            };

            // Prepare ViewModel
            var viewModel = new HomePageViewModel
            {
                Genres = genres,
                CategorizedBooks = categorizedBooks
            };

            return View(viewModel);
        }

    }
}
