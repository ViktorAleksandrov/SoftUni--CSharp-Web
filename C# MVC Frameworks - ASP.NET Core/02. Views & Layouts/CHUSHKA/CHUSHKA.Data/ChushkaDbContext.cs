using CHUSHKA.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CHUSHKA.Data
{
    public class ChushkaDbContext : IdentityDbContext<User>
    {
        public ChushkaDbContext(DbContextOptions<ChushkaDbContext> options)
               : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }
    }
}
