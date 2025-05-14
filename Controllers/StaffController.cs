using E_Library.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Library.Controllers
{
    [Authorize(Roles = "Staff")]
    public class StaffController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StaffController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard() => View();

        public async Task<IActionResult> ClaimLog()
        {
            var claims = await _context.ClaimAuditLogs
                .Include(c => c.Order)
                .Include(c => c.Staff)
                .OrderByDescending(c => c.ClaimedAt)
                .ToListAsync();

            return View(claims);
        }
    }

}
