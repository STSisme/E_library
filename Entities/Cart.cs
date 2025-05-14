using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Library.Model
{
    [Table("Cart")]
    public class Cart
    {
        [Key]
        public Guid Cart_Id { get; set; } = Guid.NewGuid();

        [Required]
        public string User_Id { get; set; }

        [Required]
        public Guid Book_Id { get; set; }

        [ForeignKey(nameof(User_Id))]
        public ApplicationUser User { get; set; }

        [ForeignKey(nameof(Book_Id))]
        public Book Book { get; set; }

        public int Quantity { get; set; }
    }
}
