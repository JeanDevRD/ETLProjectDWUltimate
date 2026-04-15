using ETLProjectDW.Core.Domain.Entities.Dims;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ETLProject.Infrastructure.Persistence.EntityConfiguration
{
    public class DimTimeEntityConfiguration : IEntityTypeConfiguration<DimTime>
    {

        public void Configure(EntityTypeBuilder<DimTime> e)
        {
            e.ToTable("DimTime");
            e.HasKey(t => t.Id);
            e.Property(t => t.Id).HasColumnName("TimeID").ValueGeneratedNever();
            e.Property(t => t.FullDate).HasColumnType("DATE");
            e.Property(t => t.Year).HasColumnName("Year");
            e.Property(t => t.Quarter).HasColumnName("Quarter");
            e.Property(t => t.Month).HasColumnName("Month");
            e.Property(t => t.MonthName).HasColumnName("MonthName").HasMaxLength(20);
            e.Property(t => t.DayOfMonth).HasColumnName("DayOfMonth");
            e.Property(t => t.DayName).HasColumnName("DayName").HasMaxLength(15);
            e.Property(t => t.IsWeekend).HasColumnName("IsWeekend");
            e.HasIndex(t => t.FullDate).IsUnique();
            e.Ignore(t => t.FactSales);
        }
    }
}
