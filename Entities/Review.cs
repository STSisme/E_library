using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elibrary.Model
{
    [Table("Review")]
    public class Review
    {
        [Key]
        public Guid Review_Id { get; set; } = Guid.NewGuid();

        public Guid User_Id { get; set; }
        public Guid Book_Id { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public User User { get; set; }
        public Book Book { get; set; }
    }

}
