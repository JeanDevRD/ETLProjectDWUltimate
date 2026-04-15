using ETLProjectDW.Core.Domain.Entities.Fact;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ETLProject.Infrastructure.Persistence.EntityConfiguration
{
    public class FactSaleEntityConfiguration : IEntityTypeConfiguration<FactSale>
    {
        public void Configure(EntityTypeBuilder<FactSale> e) 
        {
            e.ToTable("FactSales");
            e.HasKey(f => f.Id);
            e.Property(f => f.Id).HasColumnName("SaleID")
                .UseIdentityColumn();
            e.Property(f => f.UnitPrice).HasColumnType("DECIMAL(10,2)");
            e.Property(f => f.Discount).HasColumnType("DECIMAL(5,2)");
            e.Property(f => f.TotalAmount).HasColumnType("DECIMAL(10,2)");

            e.Ignore(f => f.Time);
            e.Ignore(f => f.Product);
            e.Ignore(f => f.Customer);
            e.Ignore(f => f.Status);
        }
    }
}
