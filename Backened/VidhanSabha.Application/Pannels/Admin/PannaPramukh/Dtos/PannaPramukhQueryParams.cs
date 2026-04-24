using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos
{
    public class PannaPramukhQueryParams:BaseQueryParams
    {
        public int? Id { get; set; }

        public string? UserId { get; set; }

        public int? BoothId { get; set; }


    }
}
