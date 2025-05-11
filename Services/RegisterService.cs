using E_Library.Entities;
using E_Library.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace E_Library.Services
{
    public class RegisterService
    {
 
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task RegisterUserAsync(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // User created successfully
            }
        }
     

    }
}
