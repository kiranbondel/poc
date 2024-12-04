using Microsoft.EntityFrameworkCore;
using Cfmg.Cafe.Manager.Domain.DomainModels;
using Cfmg.Cafe.Manager.Infrastructure.EntityConfiguration;

namespace Cfmg.Cafe.Manager.Infrastructure
{
    public class CafeManagerDbContext : DbContext
    {
        public CafeManagerDbContext(DbContextOptions<CafeManagerDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CafeEntityConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeEntityConfiguration());
            
        }

        public DbSet<CafeEntity> Cafe { get; set; } = null!;
        public DbSet<EmployeeEntity> Employee { get; set; } = null!;

    }
}
