using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Dtos;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Query
{
    public class getAllStatePrabhariQuery : IRequest<IReadOnlyList<StatePrabhariResponseDto>>
    {
    }
}
