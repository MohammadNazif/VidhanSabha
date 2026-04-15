using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.State.Dto;

namespace VidhanSabha.Application.Common.State.Query
{
    public class getAllStateQuery : IRequest<IReadOnlyList<StateResponseDto>>
    {
    }
}
