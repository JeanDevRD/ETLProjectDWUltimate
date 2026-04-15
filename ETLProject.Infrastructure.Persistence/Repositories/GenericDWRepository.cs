using ETLProject.Infrastructure.Persistence.DbContext;
using ETLProjectDW.Core.Application.Interfaces;
using EFCore.BulkExtensions;

namespace ETLProject.Infrastructure.Persistence.Repositories
{
    public class GenericDWRepository<TEntity> : IDataLoader<TEntity>
       where TEntity : class
    {
        protected readonly VentasDbDWContext _db;

        public GenericDWRepository(VentasDbDWContext db)
        {
            _db = db;
        }

        public virtual async Task LoadAsync(IEnumerable<TEntity> data, CancellationToken ct = default)
        {
            var list = data.ToList();
            if (!list.Any()) return;

            await _db.BulkInsertAsync(list, cancellationToken: ct);
        }
    }
}
