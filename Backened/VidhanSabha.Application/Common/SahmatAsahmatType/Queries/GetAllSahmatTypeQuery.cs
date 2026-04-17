using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.SahmatAsahmatType.DTOs;

namespace VidhanSabha.Application.Common.SahmatAsahmatType.Queries
{
    public class GetAllSahmatTypeQuery:IRequest<List<SahmatTypeResponseDto>>
    {
    }
}
