using Microsoft.EntityFrameworkCore;
using PANDA.App.Models;

namespace PANDA.App.Data
{
    public class PandaDbContext : DbContext
    {
        public DbSet<Package> Packages { get; set; }

        public DbSet<Receipt> Receipts { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=VIKTOR-PC\\SQLEXPRESS;Database=Panda;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Package>()
                 .HasOne(p => p.Recipient)
                 .WithMany(u => u.Packages)
                 .HasForeignKey(p => p.RecipientId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Receipt>()
                .HasOne(r => r.Recipient)
                .WithMany(u => u.Receipts)
                .HasForeignKey(r => r.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
