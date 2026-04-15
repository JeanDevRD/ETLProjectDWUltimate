using ETLProjectDW.Core.Domain.Entities.Dims;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ETLProject.Infrastructure.Persistence.EntityConfiguration
{
    public class DimCustomerEntityConfiguration : IEntityTypeConfiguration<DimCustomer>
    {
        public void Configure(EntityTypeBuilder<DimCustomer> e)
        {
            e.ToTable("DimCustomer");
            e.HasKey(c => c.Id);
            e.Property(c => c.Id).HasColumnName("CustomerID").ValueGeneratedNever();
            e.Property(c => c.FirstName).HasMaxLength(50);
            e.Property(c => c.LastName).HasMaxLength(50);
            e.Property(c => c.Email).HasMaxLength(100);
            e.Property(c => c.Phone).HasMaxLength(20).IsRequired(false);
            e.Property(c => c.City).HasMaxLength(100);
            e.Property(c => c.Country).HasMaxLength(100);
            e.HasIndex(c => c.Email).IsUnique();
            e.Ignore(c => c.FactSales);
        }
    }
}
