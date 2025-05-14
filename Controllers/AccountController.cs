using Microsoft.AspNetCore.Mvc;
using E_Library.Dtos;
using E_Library.Model;
using E_Library.Services.Interface;
using System.Linq;
using System;
using Microsoft.AspNetCore.Identity;

namespace E_Library.Controllers
{

    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }


        // GET Register Page
        [HttpGet]
        public IActionResult Register()
        {
            return View(new InsertUserDto()); // Initialize the model when rendering the page
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] InsertUserDto model)
        {
            Console.WriteLine("🚀 Register POST triggered");

            if (!TryValidateModel(model))
            {
                Console.WriteLine("❌ Model validation failed:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    Console.WriteLine("  • " + error.ErrorMessage);

                return View(model);
            }

            var result = await _userService.AddUserAsync(model);

            if (!result.Succeeded)
            {
                Console.WriteLine("❌ User creation failed:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine("  • " + error.Description);
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            Console.WriteLine("✅ User registered successfully");

            TempData["RegisterSuccess"] = "Registration successful! You can now log in.";

            return RedirectToAction("Login", "Account");
        }




        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginUserDto model)
        {
            if (!TryValidateModel(model)) return View(model);

            var valid = await _userService.ValidateUserAsync(model.Email, model.Password);

            if (valid)
            {
                var user = await _userService.GetUserByEmailAsync(model.Email);
                await _userService.SignInUserAsync(user, model.RememberMe);

                // 🚀 Role-based redirect
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                    return RedirectToAction("Dashboard", "Admin");
                else if (await _userManager.IsInRoleAsync(user, "Staff"))
                    return RedirectToAction("Dashboard", "Staff");
                else
                    return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _userService.SignOutUserAsync();
            return RedirectToAction("Login", "Account");
        }


    }
}
