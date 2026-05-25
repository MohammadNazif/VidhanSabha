using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Activity.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.Activity.Query
{
    
        public record GetAllActivityQuery(ActivityQueryParams qp) : IRequest<PagedResult<ActivityResponseDto>>;
    
}
