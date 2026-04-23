using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Auth.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string userId, string mobile, string role);
    }
}
