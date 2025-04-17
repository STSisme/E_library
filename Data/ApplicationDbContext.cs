using Microsoft.EntityFrameworkCore;
using Elibrary.Model;

namespace Elibrary.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

    
    }
}
