using Microsoft.AspNetCore.Mvc;
using E_Library.Data;
using E_Library.Dtos;
using E_Library.Model;
using System;
using System.Linq;

namespace E_Library.Controllers
{
    [ApiController]
    [Route("api/books")] 
    public class BookController : ControllerBase
    {
        private readonly E_LibraryDbContext _context;

        public BookController(E_LibraryDbContext context)
        {
            _context = context;
        }

        // POST: api/books/add
        [HttpPost("add")]
        public IActionResult AddBook([FromBody] InsertBookDto bookDto)
        {
            var book = new Book
            {
                Book_Id = Guid.NewGuid(),
                Title = bookDto.Title,
                Author = bookDto.Author,
                Description = bookDto.Description,
                Price = bookDto.Price,
                Stock = bookDto.Stock
            };

            _context.Books.Add(book);
            _context.SaveChanges();

            return Ok("Book Added Successfully");
        }

        // GET: api/books
        [HttpGet]
        public IActionResult GetPaginatedBooks(int page = 1, int pageSize = 10)
        {
            var books = _context.Books
                .Select(b => new
                {
                    b.Book_Id,
                    b.Title,
                    b.Author,
                    b.Description,
                    b.Price,
                    b.Stock
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalCount = _context.Books.Count();

            return Ok(new
            {
                Books = books,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            });
        }
    }
}
