using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.Influencer.DTOs
{
    public class InfluencerQueryParams : BaseQueryParams
    {
        
            public int? Id { get; set; }



            public int? BoothId { get; set; }

    }
}
