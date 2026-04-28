using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Dtos;

namespace VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Query
{
    public class getAllVidhanSabhaQuery : IRequest<IReadOnlyList<VidhanSabhaSatewiseResponseDto>>
    {
        public vidhansabhaparams q;
        public getAllVidhanSabhaQuery(vidhansabhaparams q)
        {
            this.q = q;
        }
    }
}
