using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Command
{
    public class DeleteBoothCommand : IRequest
    {
        public int Id { get; set; }
        public DeleteBoothCommand(int id)
        {
            Id = id;
        }
    }
}
