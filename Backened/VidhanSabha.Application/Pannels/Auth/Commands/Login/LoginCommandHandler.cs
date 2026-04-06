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

        public LoginCommandHandler(ILoginRepository repo)
        {
            _repo = repo;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            // Step 1: Fetch entity from DB by mobile
            var user = await _repo.GetByMobileAsync(command.MobileNumber);

            if (user == null)
                throw new UnauthorizedAccessException("Mobile number not found.");

            // Step 2: Ask domain entity if it can login (Status check)
            if (!user.CanLogin())
                throw new UnauthorizedAccessException("Account is inactive. Contact admin.");

            // Step 3: Ask domain entity to verify password (BCrypt inside entity)
            if (!user.VerifyPassword(command.Password))
                throw new UnauthorizedAccessException("Incorrect password.");

            // Step 4: Return role from entity
            return new LoginResponseDto
            {
                UserId = user.UserId,
                MobileNumber = user.MobileNumber,
                Role = user.Role,
                Status = user.Status
            };
        }
    }
}