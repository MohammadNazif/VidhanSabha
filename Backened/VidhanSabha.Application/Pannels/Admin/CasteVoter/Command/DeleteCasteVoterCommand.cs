using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.CasteVoter.Command
{
    public class DeleteCasteVoterCommand : IRequest<int>
    {
        public int Id { get; set; }

        public DeleteCasteVoterCommand(int id)
        {
            Id = id;
        }
    }
}
