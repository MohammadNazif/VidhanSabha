using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using VidhanSabha.Application.Pannels.Auth.DTOs;
using VidhanSabha.Application.Pannels.Auth.Interfaces;
using VidhanSabha.Domain.Entities.Auth;

namespace VidhanSabha.Application.Pannels.Auth.Commands.Login
{

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly ILoginRepository _repo;
        private readonly IJwtService _jwtService;
        private readonly JwtSettings _jwtSettings;

        public LoginCommandHandler(
            ILoginRepository repo,
            IJwtService jwtService,
            
            IOptions<JwtSettings> jwtSettings)
        {
            _repo = repo;
            _jwtService = jwtService;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<LoginResponseDto> Handle(
            LoginCommand command, CancellationToken cancellationToken)
        {
            // 1. Validate user
            var user = await _repo.GetByMobileAsync(command.MobileNumber);

            if (user == null)
                throw new UnauthorizedAccessException("Mobile number not found.");

            if (user.Password != command.Password)
                throw new UnauthorizedAccessException("Invalid password.");

            // 2. Cleanup old tokens
            await _repo.DeleteExpiredTokensAsync(user.UserId);

            // 3. Generate tokens
            var accessToken = _jwtService.GenerateToken(
                user.UserId, user.Mobile, user.Role.ToString());

            var refreshToken = _jwtService.GenerateRefreshToken();

            // 4. Save refresh token
            await _repo.AddRefreshTokenAsync(new Tbl_RefreshToken
            {
                Token = refreshToken,
                UserId = user.UserId,
                DeviceType = command.DeviceType,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays)
            });

            // 5. Return response
            return new LoginResponseDto
            {
                UserId = user.UserId,
                MobileNumber = user.Mobile,
                Role = user.Role,
                Token = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes)
            };
        }
    }
}