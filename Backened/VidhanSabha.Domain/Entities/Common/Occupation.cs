using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Domain.Entities.Common
{
    public class Tbl_Occupation
    {
        private string _occupation;
        public int Id { get; private set; }
        public string Occupation { get=>_occupation; private set=>_occupation=value; }
        public bool Status { get;private set;  }
    }
}
