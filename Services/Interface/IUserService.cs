using E_Library.Dtos;
using E_Library.Model;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace E_Library.Services.Interface
{
    public interface IUserService
    {
        Task<IdentityResult> AddUserAsync(InsertUserDto userDto);
        Task<bool> ValidateUserAsync(string email, string password);
        Task SignInUserAsync(ApplicationUser user, bool isPersistent = false);
        Task SignOutUserAsync();
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<ApplicationUser> GetUserByIdAsync(string userId);
    }
}
