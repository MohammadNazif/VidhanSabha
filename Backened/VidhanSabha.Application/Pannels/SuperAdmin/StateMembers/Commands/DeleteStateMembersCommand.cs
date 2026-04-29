using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.Commands
{
    public class DeleteStateMembersCommand : IRequest<int>
    {
        public int Id { get; set; }
        public DeleteStateMembersCommand(int id)
        {
            Id = id;
        }
    }
}
