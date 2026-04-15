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
    public class ProductTransformer : ITransformer<ProductDto, DimProduct>
    {
        public IEnumerable<DimProduct> Transform(IEnumerable<ProductDto> source)
        {
            var result = new List<DimProduct>();

            foreach (var p in source)
            {
                if (string.IsNullOrWhiteSpace(p.ProductName) ||
                    string.IsNullOrWhiteSpace(p.Category) ||
                    p.Price <= 0 ||
                    p.Stock < 0)
                    continue;

                result.Add(new DimProduct
                {
                    Id = p.ProductID,
                    ProductName = p.ProductName.Trim(),
                    Category = p.Category.Trim(),
                    ListPrice = p.Price,
                    Stock = p.Stock
                });
            }

            return result;
        }
    }
}
