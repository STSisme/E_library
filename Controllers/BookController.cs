using Microsoft.AspNetCore.Mvc;
using E_Library.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using E_Library.Services.Interface;
using E_Library.ViewModels;

public class BooksController : Controller
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    // GET: /Books
    public IActionResult Index(int page = 1)
    {
        const int pageSize = 10;

        // Get paginated books
        var books = _bookService.GetBooks(page, pageSize);

        // Get total count
        var totalBooks = _bookService.GetTotalBookCount();
        var totalPages = (int)Math.Ceiling(totalBooks / (double)pageSize);

        var model = new BookListViewModel
        {
            Books = books,
            TotalPages = totalPages,
            CurrentPage = page
        };

        return View(model);
    }

    // GET: /Books/Details/{id}
    public async Task<IActionResult> Details(Guid id) 
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book == null)
        {
            return NotFound();
        }

        var reviews = _bookService.GetReviewsForBook(id);

        var model = new BookDetailsViewModel
        {
            Book = book,
            Reviews = reviews
        };

        return View(model);
    }
}
