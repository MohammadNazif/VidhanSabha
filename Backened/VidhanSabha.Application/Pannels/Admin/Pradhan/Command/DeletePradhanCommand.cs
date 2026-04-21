using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.Pradhan.Command
{
    public class DeletePradhanCommand : IRequest<int>
    {
        public int Id { get; set; }
        public DeletePradhanCommand(int id)
        {
            Id = id;
        }
    }
}
