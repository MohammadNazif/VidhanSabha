using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Commands
{
    public class DeleteMandalCommand : IRequest<int>
    {
        public int Id { get; }
        public DeleteMandalCommand(int id)
        {
            Id = id;
        }
    }
}
