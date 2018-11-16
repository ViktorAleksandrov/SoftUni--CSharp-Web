using Microsoft.EntityFrameworkCore;
using TORSHIA.Models;

namespace TORSHIA.Data
{
    public class TorshiaDbContext : DbContext
    {
        public DbSet<Report> Reports { get; set; }

        public DbSet<Task> Tasks { get; set; }

        public DbSet<TaskSector> TaskSectors { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=VIKTOR-PC\\SQLEXPRESS;Database=Torshia;Integrated Security=True");
        }
    }
}
