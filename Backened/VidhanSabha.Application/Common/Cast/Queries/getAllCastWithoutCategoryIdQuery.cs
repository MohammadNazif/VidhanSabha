using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Cast.DTOs;

namespace VidhanSabha.Application.Common.Cast.Queries
{
    public class getAllCastWithoutCategoryIdQuery : IRequest<List<CastResponseDto>>
    {
        
    }
}