using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.multiplefilterparse;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos
{
    public class PannaPramukhQueryParams:BaseQueryParams
    {
        public int? Id { get; set; }
        public bool rolefilterflag { get; set; }

        public string? VillageIds { get; set; }
        public string? BoothIds { get; set; }

        public string? Role { get; set; }

        public List<int> GetVillageIds() => FilterParse.ParseIds(VillageIds);
        public List<int> GetBoothIds() => FilterParse.ParseIds(BoothIds);

        public int? BoothId { get; set; }


    }
}
