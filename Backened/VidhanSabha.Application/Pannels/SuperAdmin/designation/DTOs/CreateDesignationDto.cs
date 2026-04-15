using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.SuperAdmin.designation.DTOs
{
    public sealed  class CreateDesignationDto
    {
        public string DesignationName { get; set; } = string.Empty;
        public int DesignationTypeId { get; set; }

        //public string DesignationType { get; set; }
    }

    public sealed class DesignationResponseDto
    {
        public int Id { get; init; }
        public string DesignationName { get; init; } = string.Empty;
        public int DesignationTypeId { get; init; }
        public string DesignationTypeName { get; init; } = string.Empty;   // navigation se populate hoga
        //public bool Status { get; init; }
    }

    public sealed class UpdateDesignationDto
    {
        public int Id { get; set; }
        public string DesignationName { get; init; } = string.Empty;
        public int DesignationTypeId { get; init; }
    }
}
