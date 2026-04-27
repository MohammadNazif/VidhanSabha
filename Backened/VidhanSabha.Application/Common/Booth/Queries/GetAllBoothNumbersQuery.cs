using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Booth.Dtos;

namespace VidhanSabha.Application.Common.Booth.Queries
{
    public class GetAllBoothNumbersQuery : IRequest<List<BoothNumberDto>>
    {
        public string userId { get; set; }
        public GetAllBoothNumbersQuery(string userId)
        {
            this.userId = userId;
        }
    }
}
