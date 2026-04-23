using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.DTOs
{
    public class MandalQueryParams:BaseQueryParams
    {
        public int? Id { get; set; }
        public int? SectorId { get; set; }
        public int? CastId { get; set; }
        public int? VillageId { get; set; }
    }
}
