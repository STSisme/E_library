using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Library.Model
{
    [Table("Cart")]
    public class Cart
    {
        [Key]
        public Guid Cart_Id { get; set; } = Guid.NewGuid();

        public Guid User_Id { get; set; }
        public Guid Book_Id { get; set; }

        public int Quantity { get; set; }

        // Navigation
        public User User { get; set; }
        public Book Book { get; set; }
    }

}
