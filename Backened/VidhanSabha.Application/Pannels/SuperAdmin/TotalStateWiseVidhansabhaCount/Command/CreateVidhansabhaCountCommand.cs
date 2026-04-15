using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using static VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Dtos.VidhanshabhaStateWiseCount;

namespace VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Command
{
    public class CreateVidhansabhaCountCommand : IRequest<int>
    {
        public VidhansabhaRequestDto Dto { get; set; }
        public CreateVidhansabhaCountCommand(VidhansabhaRequestDto dto)
        {
            Dto = dto;
        }
    }
}
