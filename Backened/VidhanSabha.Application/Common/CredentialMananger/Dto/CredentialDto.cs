using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Application.Common.CredentialMananger.Dto
{
    public class CredentialDto
    {
        public string LoginId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; }
        public string Password { get; set; }
        public PrabhariRole Role { get; set; }
    }
}
