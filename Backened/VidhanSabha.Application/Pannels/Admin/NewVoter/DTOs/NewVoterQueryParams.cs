using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.multiplefilterparse;

namespace VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs
{
    public class NewVoterQueryParams:BaseQueryParams
    {
     
   

        public int? Id { get; set; }
        public string? BoothIds { get; set; }
        public string? CastIds { get; set; }
        public string? VillageIds { get; set; }

        public List<int> GetBoothIds() => FilterParse.ParseIds(BoothIds);
        public List<int> GetCastIds() => FilterParse.ParseIds(CastIds);

        public List<int> GetVillageIds() => FilterParse.ParseIds(VillageIds);
    }
}
