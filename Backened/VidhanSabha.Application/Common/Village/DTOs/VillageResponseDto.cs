using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Common.Village.DTOs
{
    public class VillageResponseDto
    {
        public int Id { get; set; }


        public int MandalId { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
    }

    public class VillageByBoothResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
