using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.DTOs
{
    public class MandalResponseDto
    {
        public int Id { get; set; }
        public int VidhanId { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
    }

    public class CreateMandalRequestDto
    {
        public int VidhanId { get; set; }
        public string Name { get; set; }
    }
}
