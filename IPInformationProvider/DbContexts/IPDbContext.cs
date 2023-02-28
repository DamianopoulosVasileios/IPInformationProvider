using IPInformationProvider.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

namespace IPInformationProvider.API.DBContext
{
    public class IPDbContext : DbContext
    {
        public IPDbContext(DbContextOptions<IPDbContext> options) : base(options) { }
        public DbSet<IP> IP { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }

    public class IPConfiguration : IEntityTypeConfiguration<IP>
    {
        public void Configure(EntityTypeBuilder<IP> builder)
        {
            builder.ToTable("IP");
            builder.HasKey(x => x.IPAddress);
            builder.Property(x => x.IPAddress).IsRequired();
        }
    }
}
