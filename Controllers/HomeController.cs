using Microsoft.AspNetCore.Mvc;
using E_Library.Data;
using E_Library.Dtos;
using E_Library.Model;
using System;
using System.Linq;

namespace E_Library.Controllers
{
    [ApiController]
    [Route("api/home")]
    public class HomeController : ControllerBase
    {
        private readonly E_LibraryDbContext _context;
        public HomeController(E_LibraryDbContext context)
        {
            _context = context;
        }

        // POST: api/home/add
        [HttpPost("add")]
        public IActionResult AddUser([FromBody] InsertUserDto userDto)
        {
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

        // GET: api/home
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

        // Additional Entities (basic listing endpoints)

        [HttpGet("books")]
        public IActionResult GetAllBooks()
        {
            var books = _context.Books.ToList();
            return Ok(books);
        }

        [HttpGet("reviews")]
        public IActionResult GetAllReviews()
        {
            var reviews = _context.Reviews.ToList();
            return Ok(reviews);
        }

        [HttpGet("orders")]
        public IActionResult GetAllOrders()
        {
            var orders = _context.Orders.ToList();
            return Ok(orders);
        }

        [HttpGet("orderitems")]
        public IActionResult GetAllOrderItems()
        {
            var items = _context.OrderItems.ToList();
            return Ok(items);
        }

        [HttpGet("carts")]
        public IActionResult GetAllCarts()
        {
            var carts = _context.Carts.ToList();
            return Ok(carts);
        }
    }
}
