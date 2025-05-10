using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Library.Model
{
    [Table("Book")]
    public class Book
    {
        [Key]
        public Guid Book_Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Title { get; set; }

        public string Author { get; set; }

        public string Genre { get; set; }

        public Guid Category_Id { get; set; } = Guid.NewGuid();

        public string Category { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string ImageUrl { get; set; }

        public DateTime PublishedDate { get; set; } 


        // Navigation Properties
        public ICollection<Review> Reviews { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Wishlist> Wishlists { get; set; }
    }

}
