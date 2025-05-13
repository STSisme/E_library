using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_Library.Data;
using E_Library.Model;

namespace E_Library.Controllers.Admin
{
    public class InventoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Inventories
        public async Task<IActionResult> Index()
        {
            var inventories = await _context.Inventory.Include(i => i.Book).ToListAsync();
            return View(inventories);
        }

        // GET: Inventories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var inventory = await _context.Inventory
                .Include(i => i.Book)
                .FirstOrDefaultAsync(m => m.Book_Id == id);

            if (inventory == null)
                return NotFound();

            return View(inventory);
        }

        // GET: Inventories/Create
        public IActionResult Create()
        {
            ViewData["Book_Id"] = new SelectList(_context.Books, "Book_Id", "Title");
            return View();
        }

        // POST: Inventories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Book_Id,Stock")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Book_Id"] = new SelectList(_context.Books, "Book_Id", "Title", inventory.Book_Id);
            return View(inventory);
        }

        // GET: Inventories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory == null)
                return NotFound();

            ViewData["Book_Id"] = new SelectList(_context.Books, "Book_Id", "Title", inventory.Book_Id);
            return View(inventory);
        }

        // POST: Inventories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Book_Id,Stock")] Inventory inventory)
        {
            if (id != inventory.Book_Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.Book_Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Book_Id"] = new SelectList(_context.Books, "Book_Id", "Title", inventory.Book_Id);
            return View(inventory);
        }

        // GET: Inventories/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var inventory = await _context.Inventory
                .Include(i => i.Book)
                .FirstOrDefaultAsync(m => m.Book_Id == id);

            if (inventory == null)
                return NotFound();

            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory != null)
                _context.Inventory.Remove(inventory);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryExists(Guid id)
        {
            return _context.Inventory.Any(e => e.Book_Id == id);
        }
    }
}
