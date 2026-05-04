using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.BoothSamiti.Command
{
    public class CreateBoothSamitiMemCommand : IRequest<int>
    {
        public int BoothId { get; set; }
        public string? UserId { get; set; }
        public string Role { get; set; }
        public CreateBoothSamitiMemCommand(int boothId, string? userId, string role)
        {
            BoothId = boothId;
            UserId = userId;
            Role = role;
        }
    }
}
