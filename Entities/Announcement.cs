using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elibrary.Model
{
    [Table("Announcement")]
    public class Announcement
    {
        [Key]
        public Guid Announcement_Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Title { get; set; }

        public string Message { get; set; }

        public DateTime PublishedDate { get; set; } = DateTime.UtcNow;
    }
}
