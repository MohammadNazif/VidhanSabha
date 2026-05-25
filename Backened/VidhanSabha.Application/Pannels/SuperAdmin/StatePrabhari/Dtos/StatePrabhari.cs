using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Command;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Dtos
{
    public class StatePrabhariResponseDto
    {
        public int Id { get; set; }

        public string userId { get; set; }
        public int? StateId { get; set; }

        public int? DistrictId { get; set; }

        public string? DistrictName { get; set; }
        public string? SectorName { get; set; }
        public string? BoothName { get; set; }
        public int? BoothNumber { get; set; }

        public int? VidhanSabhaId { get; set; }

        public string? VidhanSabhaName { get; set; }
        public int? VidhanSabhaNumber { get; set; }
        public string? StateName { get; set; }
        public string PrabhariName { get; set; } = string.Empty;
        public string PrabhariEmail { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int CastId { get; set; }
        public string CastName { get; set; } = string.Empty;
        public string Education { get; set; } = string.Empty;
        public string Profession { get; set; } = string.Empty;
        public string? CurrentAddress { get; set; }
        public string? Password { get; set; }
        public string? Profile { get; set; }
    }

    public class CreatePrabhariRequestDto
    {
        public int? stateId { get; set; }

        public int? vidhanSanhaId { get; set; }

        public PrabhariRole PrabhariRole { get; set; }
        public string PrabhariName { get; set; } = string.Empty;
        public string PrabhariEmail { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int CastId { get; set; }
        public string Education { get; set; } = string.Empty;
        public string Profession { get; set; } = string.Empty;
        public string? CurrentAddress { get; set; }

        public string? CreatedByUserId { get; set; }


    }

    public class UpdatePrabhariRequestDto : CreatePrabhariRequestDto
    {
        public int Id { get; set; }
        public string  userId { get; set; }
    }

    public class QueryParams :BaseQueryParams
    {
      
    }


}
