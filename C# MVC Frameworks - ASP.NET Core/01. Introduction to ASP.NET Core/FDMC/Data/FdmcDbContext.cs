using FDMC.Models;
using Microsoft.EntityFrameworkCore;

namespace FDMC.Data
{
    public class FdmcDbContext : DbContext
    {
        public FdmcDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Cat> Cats { get; set; }
    }
}
