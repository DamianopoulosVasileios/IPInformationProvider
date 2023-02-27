using IPInformationProvider.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IPInformationProvider.API.DBContext
{
    public class IPDbContext : DbContext
    {
        public IPDbContext(DbContextOptions<IPDbContext> options) : base(options) {}
        public DbSet<IIPs> IPs { get; set; }
    }
}
