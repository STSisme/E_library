using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elibrary.Model
{
    [Table("Wishlist")]
    public class Wishlist
    {
        [Key]
        public Guid Wishlist_Id { get; set; } = Guid.NewGuid();

        public Guid User_Id { get; set; }
        public Guid Book_Id { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public User User { get; set; }
        public Book Book { get; set; }
    }

}
