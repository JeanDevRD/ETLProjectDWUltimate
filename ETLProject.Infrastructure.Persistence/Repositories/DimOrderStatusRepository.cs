using EFCore.BulkExtensions;
using ETLProject.Infrastructure.Persistence.DbContext;
using ETLProjectDW.Core.Application.Interfaces;
using ETLProjectDW.Core.Domain.Entities.Dims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETLProject.Infrastructure.Persistence.Repositories
{
    public class DimOrderStatusRepository : GenericDWRepository<DimOrderStatus>, IDimOrderStatusRepository
    {
        public DimOrderStatusRepository(VentasDbDWContext db) : base(db) { }

        public override async Task LoadAsync(IEnumerable<DimOrderStatus> data, CancellationToken ct = default)
        {
            var list = data.ToList();
            if (!list.Any()) return;

            await _db.BulkInsertOrUpdateAsync(list, cancellationToken: ct);
        }
    }
}
