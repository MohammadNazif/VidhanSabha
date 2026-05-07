using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.multiplefilterparse;

namespace VidhanSabha.Application.Pannels.Admin.BoothVoter.DTOs
{
    public class BoothVoterQueryParams:BaseQueryParams
    {
        public int? Id { get; set; }
        public int? BoothId { get; set; }
        //public int? BoothIds { get; set; }
        public int? SectorId { get; set; }

        public string? VillageIds { get; set; }
        public string? BoothIds { get; set; }

        public List<int> GetVillageIds() => FilterParse.ParseIds(VillageIds);
        public List<int> GetBoothIds() => FilterParse.ParseIds(BoothIds);
    }
}
