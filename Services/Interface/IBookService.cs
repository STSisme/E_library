using E_Library.Dtos;
using E_Library.Entities;
using E_Library.Model;

namespace E_Library.Services.Interface
{
    public interface IBookService
    {
        void AddBook(InsertBookDto bookDto);

        // Retrieves a list of books with pagination
        IEnumerable<Book> GetBooks(int page, int pageSize);

        // Gets the total count of books
        int GetTotalBookCount();

        // Retrieves a book by its ID
        Task<Book> GetBookByIdAsync(Guid id); 

        // Retrieves reviews for a specific book
        IEnumerable<Review> GetReviewsForBook(Guid bookId); 


    }
}
