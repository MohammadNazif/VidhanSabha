using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.multiplefilterparse;

namespace VidhanSabha.Application.Pannels.Admin.SeniorDisabled.DTOs
{
    public class SeniorDisabledQueryParams:BaseQueryParams
    {
        public int? Id { get; set; }
        public bool rolefilterflag { get; set; }
        public int? TypeId { get; set; }
        public string? BoothIds { get; set; }
        public string? VillageIds { get; set; }
        public string? CastIds { get; set; }


        public List<int> GetBoothIds() => FilterParse.ParseIds(BoothIds);
        public List<int> GetVillageIds() => FilterParse.ParseIds(VillageIds);

        public List<int> GetCastIds() => FilterParse.ParseIds(CastIds);
    }
}
