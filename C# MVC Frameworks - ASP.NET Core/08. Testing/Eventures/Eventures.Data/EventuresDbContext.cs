using Eventures.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Eventures.Data
{
    public class EventuresDbContext : IdentityDbContext<User>
    {
        public EventuresDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }

        public DbSet<Order> Orders { get; set; }
    }
}
