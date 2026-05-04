using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.multiplefilterparse;

namespace VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.DTOs
{
    public class SahmatAsahmatQueryParams:BaseQueryParams
    {
        public int? Id { get; set; }
        public int? BoothId { get; set; }
        public string? VillageIds { get; set; }

        public string? partyIds { get; set; }

        public List<int> GetVillageIds() => FilterParse.ParseIds(VillageIds);
        public List<int> GetParties() => FilterParse.ParseIds(partyIds);
        public int? OccupationId { get; set; }

        public string? Type { get; set; }
        public int? TypeId { get; set; }
    }
}
