using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.BoothSamitiDesignation.DTOs;

namespace VidhanSabha.Application.Common.BoothSamitiDesignation.Queries
{
    public class GetAllBoothSamitiDesignationQuery
         : IRequest<List<DesignationDto>>
    {
    }
}
