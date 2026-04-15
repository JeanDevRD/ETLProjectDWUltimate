using ETLProjectDW.Core.Domain.Entities.Dims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETLProjectDW.Core.Application.Interfaces
{
    public interface IDimCustomerRepository : IDataLoader<DimCustomer>
    {
    }
}
