using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Library.Model
{
    [Table("Review")]
    public class Review
    {
        [Key]
        public Guid Review_Id { get; set; } = Guid.NewGuid();

        [Required]
        public string User_Id { get; set; }

        [Required]
        public Guid Book_Id { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey(nameof(User_Id))]
        public ApplicationUser User { get; set; }

        [ForeignKey(nameof(Book_Id))]
        public Book Book { get; set; }
    }
}
