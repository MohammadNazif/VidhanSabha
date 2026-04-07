using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Domain.Entities.Common
{
    
        public class Tbl_Category
    {
            private string _name;

           
            public int Id { get; private set; }
            public string Name { get => _name; private set => _name = value; }
            public bool Status { get; private set; }

        }
    }

