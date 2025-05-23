﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Library.Model
{
    [Table("Book")]
    public class Book
    {
        [Key]
        public Guid Book_Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Author { get; set; }
        public string Genre { get; set; }

        public Guid Category_Id { get; set; } = Guid.NewGuid();
        public string Category { get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public DateTime PublishedDate { get; set; }

        public decimal? DiscountPrice { get; set; }     // Optional discount price
        public bool IsBestseller { get; set; }
        public bool IsAwardWinner { get; set; }
        public bool IsComingSoon { get; set; }
        public bool IsOnSale { get; set; } = false;     // Flag to display "On Sale"
        public DateTime? SaleStartDate { get; set; }    // Optional start time
        public DateTime? SaleEndDate { get; set; }      // Optional end time


        // Navigation Properties
        public ICollection<Review> Reviews { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Wishlist> Wishlists { get; set; }
        public Inventory Inventory { get; set; } // Navigation (optional)

    }
}
