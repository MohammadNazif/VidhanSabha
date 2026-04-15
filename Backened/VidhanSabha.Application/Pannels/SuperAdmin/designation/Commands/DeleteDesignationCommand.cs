using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace VidhanSabha.Application.Pannels.SuperAdmin.designation.Commands
{
    public class DeleteDesignationCommand : IRequest<int>
    {
        public int Id { get; set; }
        public DeleteDesignationCommand(int id)
        {
            Id = id;
        }
    }
}
