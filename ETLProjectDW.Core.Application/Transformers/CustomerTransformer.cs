using ETLProjectDW.Core.Application.DTOs;
using ETLProjectDW.Core.Application.Interfaces;
using ETLProjectDW.Core.Domain.Entities.Dims;

namespace ETLProjectDW.Core.Application.Transformers
{
    public class CustomerTransformer : ITransformer<CustomerDto, DimCustomer>
    {
        public IEnumerable<DimCustomer> Transform(IEnumerable<CustomerDto> source)
        {
            var result = new List<DimCustomer>();

            foreach (var c in source)
            {
                if (string.IsNullOrWhiteSpace(c.FirstName) ||
                    string.IsNullOrWhiteSpace(c.LastName) ||
                    string.IsNullOrWhiteSpace(c.Email))
                    continue;

                result.Add(new DimCustomer
                {
                    Id = c.CustomerID,
                    FirstName = c.FirstName.Trim(),
                    LastName = c.LastName.Trim(),
                    Email = c.Email.Trim().ToLower(),
                    Phone = FormatPhone(c.Phone) ?? string.Empty,
                    City = c.City?.Trim() ?? "Unknown",
                    Country = c.Country?.Trim() ?? "Unknown"
                });
            }

            return result;
        }

        private string? FormatPhone(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return null;

            var digits = new string(raw.Where(char.IsDigit).ToArray());

            if (digits.Length == 0)
                return null;

            return digits.Length == 10
                ? $"({digits[..3]}) {digits[3..6]}-{digits[6..]}"
                : digits;
        }
    }
}
