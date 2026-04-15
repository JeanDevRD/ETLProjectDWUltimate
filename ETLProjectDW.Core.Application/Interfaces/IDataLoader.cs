using ETLProjectDW.Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETLProjectDW.Core.Application.Interfaces
{
    public interface IDataLoader<TEntity> where TEntity : class
    {
        Task LoadAsync(IEnumerable<TEntity> data, CancellationToken ct = default);
    }
}
