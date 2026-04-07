using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Domain.Entities.Common
{
    public class Tbl_Cast
    {
        private string _name;
        public int Id { get; private set; }

        public int CategoryId { get; set; }
        public string CastName { get => _name; private set => _name = value; }
        public bool Status { get; private set; }
    }
}
