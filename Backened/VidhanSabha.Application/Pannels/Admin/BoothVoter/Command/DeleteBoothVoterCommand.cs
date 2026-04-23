using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.BoothVoter.Command
{
    public class DeleteBoothVoterCommand : IRequest<int>
    {
        public int Id { get; set; }
        public DeleteBoothVoterCommand(int id)
        {
            Id = id;
        }
    }
}
