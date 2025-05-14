using E_Library.Data;
using E_Library.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace E_Library.Controllers
{


    [Authorize(Roles = "Staff")]
    public class StaffClaimController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StaffClaimController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Claim() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Claim(string code)
        {
            var staff = await _userManager.GetUserAsync(User);
            if (staff == null || staff.Role != "Staff")
            {
                return Unauthorized();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.ClaimCode == code && o.Status == "Placed");

            if (order == null)
            {
                TempData["Message"] = "❌ Invalid or already processed claim code.";
                return RedirectToAction("Claim");
            }

            // ✅ Log claim in audit table
            var audit = new ClaimAuditLog
            {
                ClaimAuditLog_Id = Guid.NewGuid(),
                Order_Id = order.Order_Id,
                Staff_Id = staff.Id,
                ClaimedAt = DateTime.UtcNow
            };

            _context.ClaimAuditLogs.Add(audit);  // ✅ ADD
            order.Status = "Fulfilled";          // ✅ Update status
            await _context.SaveChangesAsync();   // ✅ SAVE

            TempData["Message"] = $"✅ Claim for Order {order.Order_Id} fulfilled by you.";
            return RedirectToAction("Claim");
        }




        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var staff = await _userManager.GetUserAsync(User);

            var claims = await _context.Orders
                .Include(o => o.User)
                .Where(o => o.IsClaimed && o.ClaimedByStaffId == staff.Id)
                .OrderByDescending(o => o.ClaimedAt)
                .ToListAsync();

            return View(claims);
        }

    }


}
