using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Dtos
{
    public class VillageInputDto
    {
        public int VillageId { get; set; }
        public bool HasAnshik { get; set; }
        
    }

    public class VillageResponseDto
    {
        public int VillageId { get; set; }

        public string VillageName { get; set; }
        public bool HasAnshik { get; set; }
    }
}
