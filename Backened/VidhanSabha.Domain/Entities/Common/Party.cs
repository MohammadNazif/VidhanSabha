using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Domain.Entities.Common
{
    public class Tbl_Party
    {
        public int Id { get; private set; }
        public string Party { get; private set; }
        public bool Status { get; private set; }
    }
}
