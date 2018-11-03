using IRunes.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace IRunes.Data
{
    public class IRunesContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }

        public DbSet<Track> Tracks { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Server=VIKTOR-PC\SQLEXPRESS;Database=IRunesDb;Integrated Security=True")
                .UseLazyLoadingProxies();
        }
    }
}
