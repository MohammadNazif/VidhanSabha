using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.DoubleVoter.Command
{
    public class DeleteDoubleVoterCommand : IRequest<int>
    {
        public int Id { get; set; }

        public DeleteDoubleVoterCommand(int id)
        {
            Id = id;
        }
    }
}
