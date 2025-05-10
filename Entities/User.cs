using E_Library.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Library.Model
{
    [Table("User")]
    public class User
    {
        [Key]
        public Guid User_Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public Guid Membership_Id { get; set; } = Guid.NewGuid();

        public string Role { get; set; }

        public string Total_Order { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
