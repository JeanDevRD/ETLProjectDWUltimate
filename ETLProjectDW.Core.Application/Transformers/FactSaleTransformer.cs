using ETLProjectDW.Core.Application.DTOs;
using ETLProjectDW.Core.Domain.Entities.Dims;
using ETLProjectDW.Core.Domain.Entities.Fact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETLProjectDW.Core.Application.Transformers
{
    public class FactSaleTransformer
    {
        public IEnumerable<FactSale> Transform(
            IEnumerable<OrderDto> orders,
            IEnumerable<OrderDetailDto> details)
        {
            var orderMap = orders.ToDictionary(o => o.OrderID);
            var result = new List<FactSale>();

            foreach (var detail in details)
            {
                if (!orderMap.TryGetValue(detail.OrderID, out var order))
                    continue;

                if (detail.Quantity <= 0 || detail.TotalPrice <= 0)
                    continue;

                var unitPrice = Math.Round(detail.TotalPrice / detail.Quantity, 2);
                var timeId = int.Parse(order.OrderDate.Date.ToString("yyyyMMdd"));

                result.Add(new FactSale
                {
                    Id = 0,
                    TimeID = timeId,
                    ProductID = detail.ProductID,
                    CustomerID = order.CustomerID,
                    StatusID = 0, 
                    OrderID = order.OrderID,
                    Quantity = detail.Quantity,
                    UnitPrice = unitPrice,
                    Discount = 0,
                    TotalAmount = Math.Round(detail.TotalPrice, 2),
                    Status = new DimOrderStatus { Id = 0, StatusName = order.Status }
                });
            }

            return result;
        }
    }
}
