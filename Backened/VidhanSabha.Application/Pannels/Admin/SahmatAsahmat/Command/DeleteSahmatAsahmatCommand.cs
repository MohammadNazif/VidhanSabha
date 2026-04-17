using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.SahmatAsahmat.Command
{
    public class DeleteSahmatAsahmatCommand:IRequest<int>
    {
        public int Id { get; set; }
        public DeleteSahmatAsahmatCommand(int id) 
        {
            Id = id;
        }

    }
}
