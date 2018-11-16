using Microsoft.EntityFrameworkCore;
using MishMash.Models;

namespace MishMash.Data
{
    public class MishMashDbContext : DbContext
    {
        public DbSet<Channel> Channels { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<TagChannel> TagChannels { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserChannel> UserChannels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=VIKTOR-PC\SQLEXPRESS;Database=MishMash;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TagChannel>().HasKey(tc => new { tc.TagId, tc.ChannelId });

            modelBuilder.Entity<UserChannel>().HasKey(uc => new { uc.UserId, uc.ChannelId });
        }
    }
}
