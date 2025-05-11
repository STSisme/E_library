using Microsoft.EntityFrameworkCore;
using E_Library.Model;
using E_Library.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace E_Library.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Inventory> Inventory { get; set; }

    }
}
