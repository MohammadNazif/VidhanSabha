using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Auth.DTOs;
using VidhanSabha.Application.Pannels.Auth.Interfaces;
using VidhanSabha.Domain.Entities;

namespace VidhanSabha.Application.Pannels.Auth.Queries.GetMobileNumber
{
    public class GetUserByMobileQueryHandler : IRequestHandler<GetUserByMobileQuery, LoginResponseDto?>
    {
        private readonly ILoginRepository _repo;

        public GetUserByMobileQueryHandler(ILoginRepository repo)
        {
            _repo = repo;
        }

        public async Task<LoginResponseDto?> Handle(GetUserByMobileQuery query, CancellationToken ct)
        {
            var user = await _repo.GetByMobileAsync(query.MobileNumber);

            if (user == null) return null;

            return new LoginResponseDto
            {
                UserId = user.UserId,
                MobileNumber = user.Mobile,
                Role = user.Role,
                Status = user.Status
            };
        }
    }
}
