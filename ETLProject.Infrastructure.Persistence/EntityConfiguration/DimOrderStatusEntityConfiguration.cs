using ETLProjectDW.Core.Domain.Entities.Dims;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ETLProject.Infrastructure.Persistence.EntityConfiguration
{
    public class DimOrderStatusEntityConfiguration : IEntityTypeConfiguration<DimOrderStatus>
    {
        public void Configure(EntityTypeBuilder<DimOrderStatus> e)
        {
            e.ToTable("DimOrderStatus");
            e.HasKey(s => s.Id);
            e.Property(s => s.Id).HasColumnName("StatusID")
                .UseIdentityColumn();           
            e.Property(s => s.StatusName).HasMaxLength(20);
            e.HasIndex(s => s.StatusName).IsUnique();
            e.Ignore(s => s.FactSales);
        }
    }
}
