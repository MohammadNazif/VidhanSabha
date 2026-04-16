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

            if(user.Password != command.Password)
                throw new UnauthorizedAccessException("Invalid password."); 

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