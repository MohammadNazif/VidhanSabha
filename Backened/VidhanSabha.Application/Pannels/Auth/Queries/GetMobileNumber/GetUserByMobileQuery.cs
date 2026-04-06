using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Auth.DTOs;

namespace VidhanSabha.Application.Pannels.Auth.Queries.GetMobileNumber
{
    public record GetUserByMobileQuery(string MobileNumber)
     : IRequest<LoginResponseDto?>;
}
