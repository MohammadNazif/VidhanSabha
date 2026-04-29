using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Common.multiplefilterparse;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Dtos
{
    public class BoothQueryParams : BaseQueryParams
    {
        // ✅ string lo — comma separated "1,2,3"
        [FromQuery(Name = "mandalIds")]
        public string? MandalIds { get; set; }

        [FromQuery(Name = "sectorIds")]
        public string? SectorIds { get; set; }

        // Helper — string ko List<int> mein convert karo
        public List<int> GetMandalIds() => FilterParse.ParseIds(MandalIds);
        public List<int> GetSectorIds() => FilterParse.ParseIds(SectorIds);


    }
}
