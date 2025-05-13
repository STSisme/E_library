using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Library.Model
{
    [Table("OrderItem")]
    public class OrderItem
    {
        [Key]
        public Guid OrderItem_Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid Order_Id { get; set; }

        [Required]
        public Guid Book_Id { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Navigation
        [ForeignKey(nameof(Order_Id))]
        public Order Order { get; set; }

        [ForeignKey(nameof(Book_Id))]
        public Book Book { get; set; }
    }
}
