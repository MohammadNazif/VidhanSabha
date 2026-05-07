using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.multiplefilterparse;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.DTOs
{
    public class MandalQueryParams:BaseQueryParams
    {
        public int? Id { get; set; }
        public string? SectorIds { get; set; }
        public string? MandalIds { get; set; }
        public string? CastIds { get; set; }
        public string? VillageIds { get; set; }

        public List<int> GetVillageIds() => FilterParse.ParseIds(VillageIds);
        public List<int> GetMandalIds() => FilterParse.ParseIds(MandalIds);
        public List<int> GetCastIds() => FilterParse.ParseIds(CastIds);
        public List<int> GetSectorIds() => FilterParse.ParseIds(SectorIds);
    }
}
