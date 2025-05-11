using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using E_Library.Entities;

namespace E_Library.Model
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public Guid Order_Id { get; set; } = Guid.NewGuid();

        [Required]
        public string User_Id { get; set; }  // Use string for User_Id to match ApplicationUser.Id

        [ForeignKey("User_Id")]
        public ApplicationUser User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public decimal TotalAmount { get; set; }

        public string Status { get; set; }

        // Navigation
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
