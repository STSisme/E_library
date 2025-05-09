using Microsoft.EntityFrameworkCore;
using E_Library.Model;

namespace E_Library.Data
{
    public class E_LibraryDbContext(DbContextOptions<E_LibraryDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Announcement> Announcements { get; set; }


    }
}
