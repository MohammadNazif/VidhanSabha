using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.Block.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.Block.Queries
{
    public class GetAllBlockNameQuery:IRequest<List<BlockNameResponse>>
    {
        public string UserId { get; set; }
        public GetAllBlockNameQuery(string userId)
        {
            UserId = userId;
        }
    }
}
