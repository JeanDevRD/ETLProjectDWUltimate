using EFCore.BulkExtensions;
using ETLProject.Infrastructure.Persistence.DbContext;
using ETLProjectDW.Core.Application.Interfaces;
using ETLProjectDW.Core.Domain.Entities.Dims;


namespace ETLProject.Infrastructure.Persistence.Repositories
{
    public class DimTimeRepository
         : GenericDWRepository<DimTime>, IDimTimeRepository
    {
        public DimTimeRepository(VentasDbDWContext db) : base(db) { }

        public override async Task LoadAsync(IEnumerable<DimTime> data, CancellationToken ct = default)
        {
            var list = data.ToList();
            if (!list.Any()) return;

            await _db.BulkInsertAsync(list, cancellationToken: ct);
        }
    }
}
