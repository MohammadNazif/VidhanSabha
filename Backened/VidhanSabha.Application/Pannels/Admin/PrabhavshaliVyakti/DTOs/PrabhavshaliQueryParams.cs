using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.multiplefilterparse;

namespace VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.DTOs
{
    public class PrabhavshaliQueryParams:BaseQueryParams
    {
        public int? Id { get; set; }
        public int? BoothId { get; set; }
        public int? designationId { get; set; }
        public string? VillageIds { get; set; }

        public string? DesignationIds { get; set; }

        public List<int> GetVillageIds() => FilterParse.ParseIds(VillageIds);
        public List<int> GetDesignationIds() => FilterParse.ParseIds(DesignationIds);
        public int? CastId { get; set; }

        
    }
}
