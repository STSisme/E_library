using E_Library.Model;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace E_Library.Entities
{
    public class ApplicationUser : IdentityUser
    {

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        public string Role { get; set; } = "Member";

        public string Total_Order { get; set; } = "0";

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Wishlist> Wishlists { get; set; }
    }
}
