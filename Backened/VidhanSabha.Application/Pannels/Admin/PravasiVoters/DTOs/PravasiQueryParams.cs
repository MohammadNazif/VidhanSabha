using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.multiplefilterparse;

namespace VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs
{
    public class PravasiQueryParams:BaseQueryParams
    {
        public int? Id { get; set; }
        public string? BoothIds { get; set; }
        public string? CastIds { get; set; }
        public string? OccupationIds { get; set; }

        public List<int> GetBoothIds() => FilterParse.ParseIds(BoothIds);
        public List<int> GetCastIds() => FilterParse.ParseIds(CastIds);

        public List<int> GetOccupationIds() => FilterParse.ParseIds(OccupationIds);
    }
}
