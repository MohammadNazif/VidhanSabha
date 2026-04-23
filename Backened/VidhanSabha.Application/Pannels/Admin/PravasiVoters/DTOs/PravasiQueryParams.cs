using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs
{
    public class PravasiQueryParams:BaseQueryParams
    {
        public int? Id { get; set; }
        public int? BoothId { get; set; }
        public int? CastId { get; set; }
        public int? OccupationId { get; set; }
    }
}
