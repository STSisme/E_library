using E_Library.Dtos;
using E_Library.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public IActionResult Dashboard() => View();

    [HttpGet]
    public IActionResult AddStaff() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddStaff(InsertUserDto model)
    {
        if (!ModelState.IsValid) return View(model);

        var staff = new ApplicationUser
        {
            UserName = model.Username,
            Email = model.Email,
            EmailConfirmed = true,
            Username = model.Username,
            Role = "Staff",
            IsActive = true
        };

        var result = await _userManager.CreateAsync(staff, model.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(staff, "Staff");
            TempData["Message"] = "✅ Staff added successfully.";
            return RedirectToAction("ViewStaff");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View(model);
    }

    public async Task<IActionResult> ViewStaff()
    {
        var staffUsers = await _userManager.GetUsersInRoleAsync("Staff");
        return View(staffUsers);
    }
}
