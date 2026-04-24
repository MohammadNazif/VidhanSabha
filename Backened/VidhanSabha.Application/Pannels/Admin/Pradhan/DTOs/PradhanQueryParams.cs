using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.Pradhan.DTOs
{
    public class PradhanQueryParams:BaseQueryParams
    {
        public int? Id { get; set; }
        public int? SectorId { get; set; }
        public int? MandalId { get; set; }
        public int? BoothId { get; set; }
    }
}
