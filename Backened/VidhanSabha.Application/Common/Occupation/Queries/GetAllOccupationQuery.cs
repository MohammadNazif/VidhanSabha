using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Occupation.DTOs;

namespace VidhanSabha.Application.Common.Occupation.Queries
{
    public class GetAllOccupationQuery:IRequest<IReadOnlyList<OccupationResponseDto>>
    {

    }
}
