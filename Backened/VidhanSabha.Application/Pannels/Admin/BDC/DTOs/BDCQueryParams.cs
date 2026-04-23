using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.BDC.DTOs
{
    public class BDCQueryParams:BaseQueryParams
    {
        public int? Id { get; set; }
        public int? PartyId { get; set; }
    }
}
