using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace VidhanSabha.Application.Pannels.Admin.Sector.Commands
{
    public class DeleteSectorCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
