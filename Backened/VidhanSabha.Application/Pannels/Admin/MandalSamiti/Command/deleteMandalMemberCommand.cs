using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace VidhanSabha.Application.Pannels.Admin.MandalSamiti.Command
{
    public class deleteMandalMemberCommand : IRequest<int>
    {
        public int Id { get; set; }
        public int MandalId { get; set; }
        public deleteMandalMemberCommand(int id, int mandalId)
        {
            Id = id;
            MandalId = mandalId;
        }
    }
}
