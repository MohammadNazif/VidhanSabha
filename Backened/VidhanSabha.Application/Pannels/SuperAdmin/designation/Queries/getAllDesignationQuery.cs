using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.SuperAdmin.designation.DTOs;

namespace VidhanSabha.Application.Pannels.SuperAdmin.designation.Queries
{
    public class getAllDesignationQuery : IRequest<IReadOnlyList<DesignationResponseDto>>
    {
    }
}
