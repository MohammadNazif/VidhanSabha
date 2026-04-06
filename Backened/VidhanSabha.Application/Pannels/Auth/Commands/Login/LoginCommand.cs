using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Auth.DTOs;

namespace VidhanSabha.Application.Pannels.Auth.Commands.Login
{
    public class LoginCommand : IRequest<LoginResponseDto>
    {
        public string MobileNumber { get; }
        public string Password { get; }

        public LoginCommand(string mobileNumber, string password)
        {
            MobileNumber = mobileNumber;
            Password = password;
        }
    }
}
