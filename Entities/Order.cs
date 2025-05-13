using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Library.Model
{
    [Table("Order")]
    public class Order
    {  
        [Key]
        public Guid Order_Id { get; set; } = Guid.NewGuid();

        [Required]
        public string User_Id { get; set; }

        [ForeignKey(nameof(User_Id))]
        public ApplicationUser User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }

        // Navigation
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
