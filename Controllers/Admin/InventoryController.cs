using E_Library.Data;
using E_Library.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Authorize(Roles = "Admin")]
public class InventoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public InventoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var inventories = await _context.Inventory.Include(i => i.Book).ToListAsync();
        return View(inventories);
    }


    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var inventory = await _context.Inventory
            .Include(i => i.Book)
            .FirstOrDefaultAsync(i => i.Book_Id == id);

        if (inventory == null)
        {
            return NotFound();
        }

        return View(inventory);
    }


    [HttpPost]
    public async Task<IActionResult> Edit(Guid Book_Id, int Stock)
    {
        var existing = await _context.Inventory.FirstOrDefaultAsync(i => i.Book_Id == Book_Id);
        if (existing == null) return NotFound();

        existing.Stock = Stock;
        await _context.SaveChangesAsync();

        TempData["Message"] = "✅ Stock updated!";
        return RedirectToAction("Index");
    }

}
