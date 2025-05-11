using E_Library.Dtos;
using E_Library.Services.Interface;
using E_Library.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace E_Library.Services
{
    public class UserServices : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserServices(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> ValidateUserAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && user.IsActive)
            {
                return await _userManager.CheckPasswordAsync(user, password);
            }
            return false;
        }

        public async Task<IdentityResult> AddUserAsync(InsertUserDto userDto)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),  
                UserName = userDto.Username,
                Email = userDto.Email,
                Role = "Member",
                Total_Order = "0",
                IsActive = true
            };

            return await _userManager.CreateAsync(user, userDto.Password);
        }

        public async Task SignInUserAsync(ApplicationUser user, bool isPersistent = false)
        {
            await _signInManager.SignInAsync(user, isPersistent);
        }

        public async Task SignOutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
    }
}
