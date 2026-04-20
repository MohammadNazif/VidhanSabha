using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Dtos
{
    public class BoothQueryParams : BaseQueryParams
    {
        public int? MandalId { get; set; }
        public int? SectorId { get; set; }
    }
}
