using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Domain.Entities.Common
{
    public class Tbl_BoothSamitiDesignation
    {
        public int Id { get; private set; }
        public string DesignationName { get; private set; }
        public bool Status { get; private set; } = true;

        private Tbl_BoothSamitiDesignation() { }
    }
}
