using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.BDC.Command
{
    public class DeleteBDCCommand : IRequest<int>
    {
        public int Id { get; set; }

        public DeleteBDCCommand(int id)
        {
            Id = id;
        }
    }
}
