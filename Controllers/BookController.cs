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
    public IActionResult Index(string searchQuery, string sortOrder, string genreFilter, int page = 1)
    {
        const int pageSize = 10;

        var books = _bookService.GetBooks(page, pageSize, searchQuery, sortOrder, genreFilter);
        var totalBooks = _bookService.GetFilteredBookCount(searchQuery, genreFilter);
        var totalPages = (int)Math.Ceiling(totalBooks / (double)pageSize);

        var genres = _bookService.GetAllGenres(); // Populate filter dropdown

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
