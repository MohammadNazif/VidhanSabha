using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.AdminDesignation.DTOs;
using VidhanSabha.Application.Common.Party.DTOs;

namespace VidhanSabha.Application.Common.AdminDesignation.Query
{
    public class GetAllAdminDesignationQuery : IRequest<List<AdminDesignationResponseDto>>
    {
    }
}
