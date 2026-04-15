using ETLProjectDW.Core.Application.DTOs;
using ETLProjectDW.Core.Application.Interfaces;
using ETLProjectDW.Core.Domain.Entities.Dims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETLProjectDW.Core.Application.Transformers
{
    public class OrderStatusTransformer : ITransformer<OrderDto, DimOrderStatus>
    {
        public IEnumerable<DimOrderStatus> Transform(IEnumerable<OrderDto> source)
        {
            return source
                .Where(o => !string.IsNullOrWhiteSpace(o.Status))
                .Select(o => o.Status.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(name => new DimOrderStatus
                {
                    Id = 0, 
                    StatusName = name
                })
                .ToList();
        }
    }
}
