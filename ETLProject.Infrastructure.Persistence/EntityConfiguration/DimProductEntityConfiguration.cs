using ETLProjectDW.Core.Domain.Entities.Dims;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ETLProject.Infrastructure.Persistence.EntityConfiguration
{
    public class DimProductEntityConfiguration : IEntityTypeConfiguration<DimProduct>
    {
        public void Configure(EntityTypeBuilder<DimProduct> e)
        {
            e.ToTable("DimProduct");
            e.HasKey(p => p.Id);
            e.Property(p => p.Id).HasColumnName("ProductID").ValueGeneratedNever();
            e.Property(p => p.ProductName).HasMaxLength(100);
            e.Property(p => p.Category).HasMaxLength(50);
            e.Property(p => p.ListPrice).HasColumnType("DECIMAL(10,2)");
            e.Property(p => p.Stock).HasColumnName("Stock");
            e.Ignore(p => p.FactSales);
        }
    }
}
