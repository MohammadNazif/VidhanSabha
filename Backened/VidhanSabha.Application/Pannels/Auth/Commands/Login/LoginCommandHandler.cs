using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Auth.DTOs;
using VidhanSabha.Application.Pannels.Auth.Interfaces;

namespace VidhanSabha.Application.Pannels.Auth.Commands.Login
{
 
        public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
        {
            private readonly ILoginRepository _repo;
            private readonly IJwtService _jwtService;

            public LoginCommandHandler(ILoginRepository repo, IJwtService jwtService)
            {
                _repo = repo;
                _jwtService = jwtService;
            }

            public async Task<LoginResponseDto> Handle(LoginCommand command, CancellationToken cancellationToken)
            {
                var user = await _repo.GetByMobileAsync(command.MobileNumber);

                if (user == null)
                    throw new UnauthorizedAccessException("Mobile number not found.");

                if (user.Password != command.Password)
                    throw new UnauthorizedAccessException("Invalid password.");

                var token = _jwtService.GenerateToken(user.UserId, user.Mobile, user.Role.ToString());

                return new LoginResponseDto
                {
                    UserId = user.UserId,
                    MobileNumber = user.Mobile,
                    Role = user.Role,
                    Token = token,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60)
                };
            }
        }
    }