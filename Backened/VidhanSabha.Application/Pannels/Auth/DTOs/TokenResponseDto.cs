using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Auth.DTOs
{
    public record TokenResponseDto(
    string AccessToken,
    string RefreshToken,
    string DeviceType,
    DateTime ExpiresAt
);
}
