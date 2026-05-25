using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Application.Pannels.Auth.DTOs
{
    public class LoginRequestDto
    {
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public string DeviceType { get; set; }
    }

    public class LoginTokenResDto
    {
        public string MobileNumber { get; set; }
        public string userId { get; set; }
        public string Role { get; set; }
    }


    // Response
    public class LoginResponseDto
    {
        public string RefreshToken { get; set; }
        public string DeviceType { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string UserId { get; set; }
        public string MobileNumber { get; set; }
        public PrabhariRole Role { get; set; }
        public string Status { get; set; }
        public string Token { get; set; }
    }

    public class RefereshTokenResponseDto
    {
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Token { get; set; }
    }
}
