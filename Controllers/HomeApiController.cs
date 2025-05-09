using Microsoft.AspNetCore.Mvc;
using E_Library.Data;
using E_Library.Model;
using System;
using System.Linq;
using E_Library.Dtos;

namespace E_Library.Controllers
{
    [ApiController]
    [Route("api/home")]
    public class HomeApiController : ControllerBase
    {
        private readonly E_LibraryDbContext _context;

        public HomeApiController(E_LibraryDbContext context)
        {
            _context = context;
        }

        // ----------------- USERS -----------------
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

        // POST: api/home/add
        [HttpPost("add")]
        public IActionResult AddUser([FromBody] InsertUserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Username) || string.IsNullOrWhiteSpace(userDto.Password))
            {
                return BadRequest("Username and Password are required.");
            }

            var user = new User
            {
                User_Id = Guid.NewGuid(),
                Username = userDto.Username,
                Password = userDto.Password,
                Membership_Id = Guid.NewGuid(),
                Total_Order = "0",
                IsActive = true
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("User Added Successfully");
        }

        // ----------------- BOOKS -----------------
        [HttpGet("books")]
        public IActionResult GetAllBooks(int page = 1, int pageSize = 5)
        {
            var totalBooks = _context.Books.Count();
            var books = _context.Books
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                page,
                totalBooks,
                books
            });
        }

        // ----------------- ORDERS -----------------
        [HttpGet("orders")]
        public IActionResult GetAllOrders()
        {
            var orders = _context.Orders
                .Select(o => new
                {
                    o.Order_Id,
                    o.User_Id,
                    o.OrderDate,
                    o.TotalAmount,
                    o.Status,
                    Items = o.OrderItems.Select(i => new
                    {
                        i.OrderItem_Id,
                        i.Book_Id,
                        i.Quantity,
                        i.UnitPrice
                    }).ToList()
                }).ToList();

            return Ok(orders);
        }

        // ----------------- REVIEWS -----------------
        [HttpGet("reviews")]
        public IActionResult GetAllReviews()
        {
            var reviews = _context.Reviews
                .Select(r => new
                {
                    r.Review_Id,
                    r.User_Id,
                    r.Book_Id,
                    r.Rating,
                    r.Comment,
                    r.CreatedAt
                }).ToList();

            return Ok(reviews);
        }
    }
}
