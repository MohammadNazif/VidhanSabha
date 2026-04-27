using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.SeniorDisabled.DTOs
{
    public class SeniorDisabledQueryParams:BaseQueryParams
    {
        public int? Id { get; set; }
        public int? TypeId { get; set; }
        public int? BoothId { get; set; }
        public int? VillageId { get; set; }
        public int? CastId { get; set; }
    }
}
