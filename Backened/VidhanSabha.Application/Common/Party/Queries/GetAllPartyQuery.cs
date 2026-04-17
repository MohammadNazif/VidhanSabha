using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Party.DTOs;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Application.Common.Party.Queries
{
    public class GetAllPartyQuery:IRequest<List<PartyResponseDto>>
    {

    }
}
