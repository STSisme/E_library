using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Library.Entities
{
    [Table("Inventory")]
    public class Inventory
    {
        [Key]
        public Guid Book_Id { get; set; } = Guid.NewGuid();
        public int Stock { get; set; }

    }
}
