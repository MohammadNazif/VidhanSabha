using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.NewVoter.Queries
{
    public class GetAllNewVoterQuery:IRequest<List<NewVoterResponseDto>>
    {

    }
}
