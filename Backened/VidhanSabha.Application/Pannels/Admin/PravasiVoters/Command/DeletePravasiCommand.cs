using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command
{
    public class DeletePravasiCommand : IRequest<int>
    {
        public int Id { get; set; }

        public DeletePravasiCommand(int id)
        {
            Id = id;
        }
    }
}
