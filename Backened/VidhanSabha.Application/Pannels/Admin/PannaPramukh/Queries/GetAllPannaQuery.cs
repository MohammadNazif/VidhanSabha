using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.PannaPramukh.Queries
{
    public class GetAllPannaQuery : IRequest<List<PannaPramukhResponseDto>>
    {
    }
}
