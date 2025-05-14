using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Library.Model
{
    [Table("Inventory")]
    public class Inventory
    {
        [Key, ForeignKey(nameof(Book))]
        public Guid Book_Id { get; set; }

        public int Stock { get; set; }

        // Navigation
        public Book Book { get; set; }
    }
}
