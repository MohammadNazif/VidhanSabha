using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Command
{
    public class DeletePrabhavCommand : IRequest<int>
    {
        public int Id { get; set; }

        public DeletePrabhavCommand(int id)
        {
            Id = id;
        }
    }
}
