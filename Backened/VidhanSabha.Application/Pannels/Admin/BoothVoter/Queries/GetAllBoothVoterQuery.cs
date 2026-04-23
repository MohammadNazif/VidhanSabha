using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.BoothVoter.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.BoothVoter.Queries
{
    public class GetAllBoothVoterQuery: IRequest<List<BoothVoterResponseDto>>
    {
    }
}
