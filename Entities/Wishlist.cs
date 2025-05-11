using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using E_Library.Entities;

namespace E_Library.Model
{
    [Table("Wishlist")]
    public class Wishlist
    {
        [Key]
        public Guid Wishlist_Id { get; set; } = Guid.NewGuid();

        [Required]
        public string User_Id { get; set; }

        [Required]
        public Guid Book_Id { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public ApplicationUser User { get; set; }

        [ForeignKey("Book_Id")]
        public Book Book { get; set; }
    }
}
