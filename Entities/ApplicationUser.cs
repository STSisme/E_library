using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_Library.Model
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Username { get; set; }   

        public string Role { get; set; } = "Member";
        public string TotalOrder { get; set; } = "0";
        public bool IsActive { get; set; } = true;

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public ICollection<Order> Orders { get; set; } = new List<Order>(); // Member's own orders
        public ICollection<Order> ClaimsProcessed { get; set; } = new List<Order>(); // Orders this staff fulfilled
        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
    }
}
