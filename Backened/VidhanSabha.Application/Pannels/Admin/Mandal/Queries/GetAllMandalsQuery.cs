using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Queries
{
    public class GetAllMandalsQuery : IRequest<List<MandalResponseDto>>
    {
    }
}
