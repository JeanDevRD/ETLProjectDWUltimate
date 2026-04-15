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
    public class TimeTransformer : ITransformer<OrderDto, DimTime>
    {
        public IEnumerable<DimTime> Transform(IEnumerable<OrderDto> source)
        {
            return source
                .Select(o => o.OrderDate.Date)
                .Distinct()
                .Select(date => new DimTime
                {
                    Id = int.Parse(date.ToString("yyyyMMdd")),
                    FullDate = date,
                    Year = (short)date.Year,
                    Month = (byte)date.Month,
                    Quarter = (byte)((date.Month - 1) / 3 + 1),
                    MonthName = date.ToString("MMMM"),
                    DayOfMonth = (byte)date.Day,
                    DayName = date.ToString("dddd"),
                    IsWeekend = date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday
                })
                .ToList();
        }
    }
}
