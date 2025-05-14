using E_Library.Data;
using E_Library.Dtos;
using E_Library.Model;
using E_Library.Services.Interface;
using Microsoft.EntityFrameworkCore;



namespace E_Library.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetBooks(int page, int pageSize)
        {
            return _context.Books
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetTotalBookCount()
        {
            return _context.Books.Count();
        }

        public IEnumerable<Book> GetBooks(int page, int pageSize, string searchQuery, string sortOrder, string genreFilter)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
                query = query.Where(b => b.Title.Contains(searchQuery) || b.Description.Contains(searchQuery));

            if (!string.IsNullOrEmpty(genreFilter))
                query = query.Where(b => b.Genre == genreFilter);

            if (sortOrder == "title")
                query = query.OrderBy(b => b.Title);
            else if (sortOrder == "date")
                query = query.OrderByDescending(b => b.PublishedDate);

            return query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetFilteredBookCount(string searchQuery, string genreFilter)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
                query = query.Where(b => b.Title.Contains(searchQuery) || b.Description.Contains(searchQuery));

            if (!string.IsNullOrEmpty(genreFilter))
                query = query.Where(b => b.Genre == genreFilter);

            return query.Count();
        }

        public IEnumerable<string> GetAllGenres()
        {
            return _context.Books
                .Select(b => b.Genre)
                .Distinct()
                .ToList();
        }


        public IEnumerable<Review> GetReviewsForBook(Guid bookId)
        {
            return _context.Reviews.Where(r => r.Book_Id == bookId).ToList();
        }

        public void AddBook(InsertBookDto bookDto)
        {
            var book = new Book
            {
                Book_Id = Guid.NewGuid(),
                Title = bookDto.Title,
                Author = bookDto.Author,
                Genre = bookDto.Genre,
                Description = bookDto.Description,
                Price = bookDto.Price,
                PublishedDate = bookDto.PublishedDate,
            };

            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public async Task<Book> GetBookByIdAsync(Guid id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Book_Id == id);
        }
    }
}