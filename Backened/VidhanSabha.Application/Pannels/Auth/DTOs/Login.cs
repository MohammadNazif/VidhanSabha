using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Auth.DTOs
{
    public class LoginRequestDto
    {
        public string MobileNumber { get; set; }
        public string Password { get; set; }
    }

    // Response
    public class LoginResponseDto
    {
        public string UserId { get; set; }
        public string MobileNumber { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public string Token { get; set; }      // <-- added
        public DateTime ExpiresAt { get; set; } // <-- added
    }
}
