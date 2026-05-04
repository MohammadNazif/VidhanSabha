using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.multiplefilterparse;

namespace VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos
{
    public class PannaPramukhQueryParams:BaseQueryParams
    {
        public int? Id { get; set; }

        public string? VillageIds { get; set; }

        public List<int> GetVillageIds() => FilterParse.ParseIds(VillageIds);

        public int? BoothId { get; set; }


    }
}
