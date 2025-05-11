using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using E_Library.Model;

namespace E_Library.Entities
{
    [Table("Inventory")]
    public class Inventory
    {
        [Key, ForeignKey("Book")] // Foreign key for Book
        public Guid Book_Id { get; set; }
        public int Stock { get; set; }

        // ✅ Navigation property
        public Book Book { get; set; }


    }
}