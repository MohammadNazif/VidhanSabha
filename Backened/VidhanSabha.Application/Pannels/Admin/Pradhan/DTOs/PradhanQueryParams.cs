using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.multiplefilterparse;

namespace VidhanSabha.Application.Pannels.Admin.Pradhan.DTOs
{
    public class PradhanQueryParams:BaseQueryParams
    {
        public int? Id { get; set; }
        public string? SectorIds { get; set; }
        public string? MandalIds { get; set; }
        public string? BoothIds { get; set; }

        public List<int> GetBoothIds() => FilterParse.ParseIds(BoothIds);
        public List<int> GetSectorIds() => FilterParse.ParseIds(SectorIds);

        public List<int> GetMandalIds() => FilterParse.ParseIds(MandalIds);

    }
}
