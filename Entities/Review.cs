using E_Library.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Library.Model
{
    [Table("Review")]
    public class Review
    {
        [Key]
        public Guid Review_Id { get; set; } = Guid.NewGuid();

        [Required]
        public string User_Id { get; set; }  // Use string for User_Id to match ApplicationUser.Id

        [Required]
        public Guid Book_Id { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("User_Id")]
        public ApplicationUser User { get; set; }

        [ForeignKey("Book_Id")]
        public Book Book { get; set; }
    }
}
