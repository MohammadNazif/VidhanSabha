using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Common.multiplefilterparse
{
    public class FilterParse
    {
        public static List<int> ParseIds(string? value) =>
          string.IsNullOrWhiteSpace(value)
              ? new List<int>()
              : value.Split(',')
                     .Select(x => int.TryParse(x.Trim(), out var id) ? id : 0)
                     .Where(x => x > 0)
                     .ToList();
    }

}
