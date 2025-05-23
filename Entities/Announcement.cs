﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Library.Model
{
    [Table("Announcement")]
    public class Announcement
    {
        [Key]
        public Guid Announcement_Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Message { get; set; }

        public DateTime PublishedDate { get; set; } = DateTime.UtcNow;
    }
}
