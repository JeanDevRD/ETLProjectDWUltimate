using ETLProjectDW.Core.Domain.Entities.Dims;
using ETLProjectDW.Core.Domain.Entities.Fact;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ETLProject.Infrastructure.Persistence.DbContext
{
    public class VentasDbDWContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<DimTime> DimTime { get; set; }
        public DbSet<DimProduct> DimProduct { get; set; }
        public DbSet<DimCustomer> DimCustomer { get; set; }
        public DbSet<DimOrderStatus> DimOrderStatus { get; set; }
        public DbSet<FactSale> FactSales { get; set; }

        public VentasDbDWContext(DbContextOptions<VentasDbDWContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);
            mb.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}