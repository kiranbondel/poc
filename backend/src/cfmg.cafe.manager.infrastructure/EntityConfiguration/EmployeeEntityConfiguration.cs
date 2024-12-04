using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cfmg.Cafe.Manager.Domain.DomainModels;

namespace Cfmg.Cafe.Manager.Infrastructure.EntityConfiguration
{
    public class EmployeeEntityConfiguration : BaseEntityConfiguration<EmployeeEntity>
    {
        override
        public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
        {
            base.Configure(builder);
            builder.HasOne(e => e.Cafe)
            .WithMany(c => c.Employees)
            .HasForeignKey(e => e.CafeId)
            .OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}
