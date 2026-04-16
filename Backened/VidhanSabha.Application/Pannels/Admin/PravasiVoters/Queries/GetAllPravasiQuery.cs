using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.PravasiVoters.Queries
{
    public class GetAllPravasiQuery:IRequest<List<PravasiVoterResponseDto>>
    {
    }
}
