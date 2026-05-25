using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.MandalSamiti.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.MandalSamiti.Query
{
    public class getAllMandalSamitiDesignationQuery : IRequest<List<MandalSamitiDesignationResponseDto>>
    {
    }
}
