using E_Library.Data;
using E_Library.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Library.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AnnouncementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var announcements = _context.Announcements.OrderByDescending(a => a.PublishedDate).ToList();
            return View(announcements);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Announcement model)
        {
            if (ModelState.IsValid)
            {
                // ✅ Ensure PublishedDate is UTC
                model.PublishedDate = DateTime.SpecifyKind(model.PublishedDate, DateTimeKind.Utc);

                _context.Announcements.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

    }

}
