using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.BoothSamiti.Command
{
    public class DeleteBoothSamitiCommand : IRequest<int>
    {
        public int Id { get; set; }

        public DeleteBoothSamitiCommand(int id)
        {
            Id = id;
        }
    }
}
