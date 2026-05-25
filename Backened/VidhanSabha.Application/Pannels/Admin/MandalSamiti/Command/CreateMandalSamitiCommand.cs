using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.MandalSamiti.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.MandalSamiti.Command
{
    public class CreateMandalSamitiCommand : IRequest<int>
    {
        public int  MandalId;
        public string UserId;
        public string Role;
        public CreateMandalSamitiCommand(int mandalId, string userId, string role)
        {
            MandalId = mandalId;
            UserId = userId;
            Role = role;
        }
    }
}
