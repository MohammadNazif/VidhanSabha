using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.NewVoter.Command
{
    public class DeleteNewVoterCommand:IRequest<int>
    {
        public int Id { get; set;}
        public DeleteNewVoterCommand(int id)
        {
            Id = id;
        }
    }
}
