using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Queries
{
    public class getAllBoothInchargeQuery : IRequest<IReadOnlyList<BoothInchargeResponse>>
    {
        public int? BoothId { get; set; }

        public getAllBoothInchargeQuery(int? boothId)
        {
            BoothId = boothId;
        }
    }
}

