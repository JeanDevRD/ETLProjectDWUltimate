using EFCore.BulkExtensions;
using ETLProject.Infrastructure.Persistence.DbContext;
using ETLProjectDW.Core.Application.Interfaces;
using ETLProjectDW.Core.Domain.Entities.Fact;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETLProject.Infrastructure.Persistence.Repositories
{
    public class FactSaleRepository
        : GenericDWRepository<FactSale>, IFactSaleRepository
    {
        public FactSaleRepository(VentasDbDWContext db) : base(db) { }

        public override async Task LoadAsync(IEnumerable<FactSale> data, CancellationToken ct = default)
        {
            var statusMap = await _db.DimOrderStatus.ToDictionaryAsync(s => s.StatusName, s => s.Id, ct);

            var validSales = new List<FactSale>();

            foreach (var sale in data)
            {
                var statusName = sale.Status?.StatusName ?? string.Empty;

                if (!statusMap.TryGetValue(statusName, out var realStatusId))
                    continue;

                sale.StatusID = realStatusId;
                sale.Status = null;

                validSales.Add(sale);
            }

            if (!validSales.Any()) return;

            await _db.BulkInsertAsync(validSales, cancellationToken: ct);
        }
    }
}
