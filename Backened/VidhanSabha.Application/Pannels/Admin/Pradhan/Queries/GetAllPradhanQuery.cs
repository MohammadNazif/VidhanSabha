using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Pradhan.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.Pradhan.Queries
{
    public class GetAllPradhanQuery:IRequest<List<PradhanResponseDto>>
    {

    }
}
