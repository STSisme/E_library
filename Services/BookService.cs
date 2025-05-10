using E_Library.Data;
using E_Library.Dtos;
using E_Library.Model;
using Microsoft.EntityFrameworkCore;
using E_Library.Services.Interface;

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

        public async Task<Book> GetBookByIdAsync(Guid id)
        {
            return await _context.Books
                .Include(b => b.Reviews)
                .FirstOrDefaultAsync(b => b.Book_Id == id);
        }

        public IEnumerable<Review> GetReviewsForBook(Guid bookId)
        {
            return _context.Reviews.Where(r => r.Book_Id == bookId).ToList();
        }

        public void AddBook(InsertBookDto bookDto)
        {
            throw new NotImplementedException();
        }

        public Task<Book> GetBookByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Review> GetReviewsForBook(int bookId)
        {
            throw new NotImplementedException();
        }
    }
}