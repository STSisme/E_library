using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elibrary.Model
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public Guid Order_Id { get; set; } = Guid.NewGuid();

        public Guid User_Id { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }

        public string Status { get; set; }

        // Navigation
        public User User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }

}
