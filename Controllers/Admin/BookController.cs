﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Library.Data;
using E_Library.Model;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

[Authorize(Roles = "Admin")]
[Route("Admin/Book")]
public class BookController : Controller
{
    private readonly ApplicationDbContext _context;

    public BookController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index(int page = 1)
    {
        int pageSize = 20;
        var totalBooks = await _context.Books.CountAsync();

        var books = await _context.Books
            .Include(b => b.Inventory)
            .OrderBy(b => b.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalBooks / (double)pageSize);

        return View("~/Views/Admin/Book/Index.cshtml", books);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View("~/Views/Admin/Book/Create.cshtml");
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePost()
    {
        try
        {
            var book = new Book
            {
                Book_Id = Guid.NewGuid(),
                Title = Request.Form["Title"].ToString(),
                Author = Request.Form["Author"].ToString(),
                Genre = Request.Form["Genre"].ToString(),
                Category = Request.Form["Category"].ToString(),
                Description = Request.Form["Description"].ToString(),
                Price = decimal.Parse(Request.Form["Price"]),
                ImageUrl = Request.Form["ImageUrl"].ToString(),
                IsBestseller = bool.Parse(Request.Form["IsBestseller"].ToString() ?? "false"),
                IsAwardWinner = bool.Parse(Request.Form["IsAwardWinner"].ToString() ?? "false"),
                IsComingSoon = bool.Parse(Request.Form["IsComingSoon"].ToString() ?? "false")
            };

            // Correct UTC kind for PostgreSQL
            DateTime.TryParse(Request.Form["PublishedDate"].ToString(),
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeLocal | DateTimeStyles.AdjustToUniversal,
                out var publishedUtc);

            book.PublishedDate = publishedUtc;

            book.DiscountPrice = string.IsNullOrEmpty(Request.Form["DiscountPrice"].ToString())
                ? null
                : decimal.Parse(Request.Form["DiscountPrice"].ToString());

            book.IsOnSale = Request.Form["IsOnSale"].ToString() == "true";

            if (DateTime.TryParse(Request.Form["SaleStartDate"].ToString(), out var start))
                book.SaleStartDate = DateTime.SpecifyKind(start, DateTimeKind.Utc);

            if (DateTime.TryParse(Request.Form["SaleEndDate"].ToString(), out var end))
                book.SaleEndDate = DateTime.SpecifyKind(end, DateTimeKind.Utc);

            int stock = int.Parse(Request.Form["Stock"]);

            _context.Books.Add(book);
            _context.Inventory.Add(new Inventory
            {
                Book_Id = book.Book_Id,
                Stock = stock
            });

            await _context.SaveChangesAsync();

            TempData["Message"] = "✅ Book saved successfully!";
            return RedirectToAction("Create"); // stay on form with message
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"❌ Failed to save book: {ex.Message}";
            return RedirectToAction("Create");
        }
    }



    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var book = await _context.Books.Include(b => b.Inventory).FirstOrDefaultAsync(b => b.Book_Id == id);
        if (book == null) return NotFound();

        return View("~/Views/Admin/Book/Edit.cshtml", book);
    }

    [HttpPost("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost()
    {
        var id = Guid.Parse(Request.Form["Book_Id"].ToString());
        var book = await _context.Books.FindAsync(id);
        if (book == null) return NotFound();

        book.Title = Request.Form["Title"].ToString();
        book.Author = Request.Form["Author"].ToString();
        book.Genre = Request.Form["Genre"].ToString();
        book.Category = Request.Form["Category"].ToString();
        book.Description = Request.Form["Description"].ToString();
        book.Price = decimal.Parse(Request.Form["Price"]);
        book.ImageUrl = Request.Form["ImageUrl"].ToString();

        DateTime.TryParse(Request.Form["PublishedDate"].ToString(), CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeLocal | DateTimeStyles.AdjustToUniversal,
            out var publishedDateUtc);

        book.PublishedDate = publishedDateUtc;

        book.DiscountPrice = string.IsNullOrEmpty(Request.Form["DiscountPrice"].ToString())
                ? null
                : decimal.Parse(Request.Form["DiscountPrice"].ToString());

        book.IsOnSale = Request.Form["IsOnSale"].ToString() == "true";

        if (DateTime.TryParse(Request.Form["SaleStartDate"].ToString(), out var start))
            book.SaleStartDate = DateTime.SpecifyKind(start, DateTimeKind.Utc);

        if (DateTime.TryParse(Request.Form["SaleEndDate"].ToString(), out var end))
            book.SaleEndDate = DateTime.SpecifyKind(end, DateTimeKind.Utc);

        int stock = int.Parse(Request.Form["Stock"]);
        var inventory = await _context.Inventory.FindAsync(id);
        if (inventory != null)
            inventory.Stock = stock;

        await _context.SaveChangesAsync();
        TempData["Message"] = "✅ Book updated!";
        return RedirectToAction("Index");
    }

    [HttpPost("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var book = await _context.Books.FindAsync(id);
        var inventory = await _context.Inventory.FindAsync(id);

        if (book != null)
            _context.Books.Remove(book);
        if (inventory != null)
            _context.Inventory.Remove(inventory);

        await _context.SaveChangesAsync();
        TempData["Message"] = "🗑️ Book deleted.";
        return RedirectToAction("Index");
    }
}
