using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Category.DTOs;

namespace VidhanSabha.Application.Common.Category.Queries
{
    public class getall : IRequest<List<CategoryResponseDto>>
    { 
    }
}
