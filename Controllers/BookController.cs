using Microsoft.AspNetCore.Mvc;
using E_Library.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using E_Library.Services.Interface;
using E_Library.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using E_Library.Data;

public class BooksController : Controller
{
    private readonly IBookService _bookService;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public BooksController(
        IBookService bookService,
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _bookService = bookService;
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Index(string searchQuery, string sortOrder, string genreFilter, int page = 1)
    {
        const int pageSize = 10;

        var books = _bookService.GetBooks(page, pageSize, searchQuery, sortOrder, genreFilter);
        var totalBooks = _bookService.GetFilteredBookCount(searchQuery, genreFilter);
        var totalPages = (int)Math.Ceiling(totalBooks / (double)pageSize);

        var genres = _bookService.GetAllGenres();

        var model = new BookListViewModel
        {
            Books = books,
            TotalPages = totalPages,
            CurrentPage = page,
            SearchQuery = searchQuery,
            SortOrder = sortOrder,
            GenreFilter = genreFilter,
            Genres = genres
        };

        return View(model);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return NotFound();

        var reviews = await _context.Reviews
            .Where(r => r.Book_Id == id)
            .ToListAsync();

        var model = new BookDetailsViewModel
        {
            Book = book,
            Reviews = reviews
        };

        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.IsWishlisted = await _context.Wishlists
                .AnyAsync(w => w.Book_Id == id && w.User_Id == user.Id);
        }
        else
        {
            ViewBag.IsWishlisted = false;
        }

        return View(model);
    }
}
