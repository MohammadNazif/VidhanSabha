using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.Block.Command
{
    public class DeleteBlockCommand : IRequest<int>
    {
        public int Id { get; set; }

        public DeleteBlockCommand(int id)
        {
            Id = id;
        }
    }
}
