using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cfmg.Cafe.Manager.Common.Library.SeedWork;

namespace Cfmg.Cafe.Manager.Infrastructure.EntityConfiguration
{
    public class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(o => o.IsActive).IsRequired();
        }
    }
}
