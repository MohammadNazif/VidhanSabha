using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using VidhanSabha.Application.Pannels.Auth.DTOs;
using VidhanSabha.Application.Pannels.Auth.Interfaces;
using VidhanSabha.Domain.Entities.Auth;

namespace VidhanSabha.Application.Pannels.Auth.Commands.Login
{
    // RefreshTokenCommandHandler.cs
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefereshTokenResponseDto>
    {
        private readonly ILoginRepository _repo;
        private readonly IJwtService _jwtService;
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenCommandHandler(
            ILoginRepository repo,
            IJwtService jwtService,
            IOptions<JwtSettings> jwtSettings)
        {
            _repo = repo;
            _jwtService = jwtService;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<RefereshTokenResponseDto> Handle(
            RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // 1. Find stored token with User (navigation property)
            var stored = await _repo.GetRefreshTokenAsync(request.RefreshToken);

            if (stored is null)
                throw new UnauthorizedAccessException("Token not found.");

            if (!stored.IsActive)
                throw new UnauthorizedAccessException("Token is expired or revoked.");

            // 2. Revoke old token
            await _repo.RevokeTokenAsync(stored.Token);

            // 3. Cleanup expired/revoked tokens for same user
            await _repo.DeleteExpiredTokensAsync(stored.UserId);

            // 4. Issue new tokens
            var newAccessToken = _jwtService.GenerateToken(
                stored.User.UserId, stored.User.Mobile, stored.User.Role.ToString());

            var newRefreshToken = _jwtService.GenerateRefreshToken();

            await _repo.AddRefreshTokenAsync(new Tbl_RefreshToken
            {
                Token = newRefreshToken,
                UserId = stored.UserId,
                DeviceType = stored.DeviceType,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays)
            });

            return new RefereshTokenResponseDto
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes)
            };
        }
    }
}
