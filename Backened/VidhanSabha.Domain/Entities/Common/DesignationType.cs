using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Domain.Entities.Common
{
    public class Tbl_DesignationType
    {
        public int Id { get; set; }
        public string DesignationName { get; set; }
        public bool Status { get; set; }
    }
}
