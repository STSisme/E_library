using E_Library.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Library.Model
{
    [Table("Cart")]
    public class Cart
    {
        [Key]
        public Guid Cart_Id { get; set; } = Guid.NewGuid();

        [ForeignKey("User_Id")]
        public string User_Id { get; set; }  // Use string for User_Id to match ApplicationUser.Id

        [Required]
        public Guid Book_Id { get; set; }

        public ApplicationUser User { get; set; } // Corrected type

        [ForeignKey("Book_Id")]
        public Book Book { get; set; }

        public int Quantity { get; set; }
    }
}
