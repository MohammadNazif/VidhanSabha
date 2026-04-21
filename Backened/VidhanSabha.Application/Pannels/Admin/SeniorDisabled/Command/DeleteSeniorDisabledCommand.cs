using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Command
{
    public class DeleteSeniorDisabledCommand : IRequest<int>
    {
        public int Id { get; set; }

        public DeleteSeniorDisabledCommand(int id)
        {
            Id = id;
        }
    }
}
