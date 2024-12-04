using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cfmg.Cafe.Manager.Domain.DomainModels;

namespace Cfmg.Cafe.Manager.Infrastructure.EntityConfiguration
{
    public class CafeEntityConfiguration: BaseEntityConfiguration<CafeEntity>
    {
        override
        public void Configure(EntityTypeBuilder<CafeEntity> builder)
        {
            base.Configure(builder);
            builder.Property(o => o.Id)
                   .HasDefaultValueSql("NEWID()")
                   .ValueGeneratedOnAdd();
            
        }
    }
}
