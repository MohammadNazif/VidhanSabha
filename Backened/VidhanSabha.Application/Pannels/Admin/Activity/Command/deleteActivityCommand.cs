using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.Activity.Command
{
    public class deleteActivityCommand:IRequest<int>
    {
        public int Id { get; set; }
        public deleteActivityCommand(int id)
        {
            Id = id;
        }
    }
}
