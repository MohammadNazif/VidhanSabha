using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.DTOs
{
    public class MandalReportDto
    {
        public int MandalId { get; set; }
        public string MandalName { get; set; }
        public int TotalSectors { get; set; }
        public int TotalBooths { get; set; }
        public int TotalVotes { get; set; }
        public int SeniorCitizen { get; set; }
        public int Handicap { get; set; }
        public int DoubleVotes { get; set; }
        public int Pravasi { get; set; }
        public int EffectivePerson { get; set; }
    }
}
