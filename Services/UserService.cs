using E_Library.Dtos;
using E_Library.Model;
using E_Library.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace E_Library.Services
{
    public class UserServices : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserServices(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> AddUserAsync(InsertUserDto userDto)
        {
            // Check if email already exists
            var existingUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == userDto.Email);

            if (existingUser != null)
            {
                Console.WriteLine($"? User with email {userDto.Email} already exists.");
                var result = IdentityResult.Failed(
                    new IdentityError { Description = "User with this email already exists." });
                return result;
            }

            var user = new ApplicationUser
            {
                UserName = userDto.Username,
                Username = userDto.Username,
                Email = userDto.Email,
                Role = "Member",
                TotalOrder = "0",
                IsActive = true
            };

            var resultCreate = await _userManager.CreateAsync(user, userDto.Password);

            if (!resultCreate.Succeeded)
            {
                foreach (var error in resultCreate.Errors)
                    Console.WriteLine("? User Creation Error: " + error.Description);
            }
            else
            {
                Console.WriteLine($"? User {userDto.Email} created successfully.");
            }

            return resultCreate;
        }

        public async Task<bool> ValidateUserAsync(string email, string password)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);

            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                return result.Succeeded;
            }

            Console.WriteLine("? Login failed: user not found or inactive.");
            return false;
        }

        public async Task SignInUserAsync(ApplicationUser user, bool isPersistent = false)
        {
            await _signInManager.SignInAsync(user, isPersistent);
            Console.WriteLine($"? User signed in: {user.Email}");
        }

        public async Task SignOutUserAsync()
        {
            await _signInManager.SignOutAsync();
            Console.WriteLine("?? User signed out");
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
    }
}
