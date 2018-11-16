using CHUSHKA.Models;
using Microsoft.EntityFrameworkCore;

namespace CHUSHKA.Data
{
    public class ChushkaDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=VIKTOR-PC\\SQLEXPRESS;Database=Chushka;Integrated Security=True");
        }
    }
}
