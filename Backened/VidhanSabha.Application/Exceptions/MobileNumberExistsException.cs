using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Exceptions
{
    public class MobileNumberExistsException : Exception
    {
        public MobileNumberExistsException(string message = "Mobile number already exists.")
            : base(message)
        {
        }
    }
}
