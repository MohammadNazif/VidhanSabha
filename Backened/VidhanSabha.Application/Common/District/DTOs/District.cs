using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Common.District.DTOs
{
    public class DistrictResponseDto
    {
        public int Id { get; set; }
        //public int StateId { get; set; }
        public string DistrictName { get; set; }
        public bool Status { get; set; }

    }
}
