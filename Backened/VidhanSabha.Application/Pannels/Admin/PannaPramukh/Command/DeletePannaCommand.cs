using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace VidhanSabha.Application.Pannels.Admin.PannaPramukh.Command
{
    public class DeletePannaCommand : IRequest<int>
    {
        public int Id { get; set; }
        public DeletePannaCommand(int id)
        {
            Id = id;
        }
    }
}
