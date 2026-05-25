using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Dtos;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Query
{
    public class getProfileQuery : IRequest<StatePrabhariResponseDto>
    {
        public string UserId { get; set; }
        public string Role { get; set; }
        public getProfileQuery(string userId, string role)
        {
            UserId = userId;
            Role = role;
        }
    }
}
