using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Booth.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Queries
{
    public class getboothbysectoridquery : IRequest<List<BoothNumberDto>>
    {
        public string UserId { get; set; }
        public getboothbysectoridquery(string userId)
        {
            UserId = userId;
        }
    }
}
