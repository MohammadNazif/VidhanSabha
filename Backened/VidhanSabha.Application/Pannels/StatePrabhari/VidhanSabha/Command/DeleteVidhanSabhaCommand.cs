using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Command
{
    public class DeleteVidhanSabhaCommand : IRequest<int>
    {
        public int Id { get; set; }
        public DeleteVidhanSabhaCommand(int id)
        {
            Id = id;
        }
    }
}
