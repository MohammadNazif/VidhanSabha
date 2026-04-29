using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.BoothVoter.DTOs
{
    public class BoothVoterQueryParams:BaseQueryParams
    {
        public int? Id { get; set; }
        public int? BoothId { get; set; }
        public int? SectorId { get; set; }
    }
}
