using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using static VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Dtos.VidhanshabhaStateWiseCount;

namespace VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Query
{
    public class getAllvidhanSabhaCountQuery : IRequest<IReadOnlyList<VidhansabhaResponseDto>>
    {
        public string? UserId;
        public getAllvidhanSabhaCountQuery(string? userId)
        {
            UserId = userId;
        }
    }
}
