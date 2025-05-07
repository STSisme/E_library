using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elibrary.Model
{
    [Table("OrderItem")]
    public class OrderItem
    {
        [Key]
        public Guid OrderItem_Id { get; set; } = Guid.NewGuid();

        public Guid Order_Id { get; set; }
        public Guid Book_Id { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Navigation
        public Order Order { get; set; }
        public Book Book { get; set; }
    }

}
