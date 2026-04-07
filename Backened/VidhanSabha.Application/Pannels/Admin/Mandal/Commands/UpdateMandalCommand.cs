using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs.Create;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Commands
{
    public class UpdateMandalCommand : IRequest<MandalResponseDto>
    {
        public int Id { get; init; }
        public string Name { get; init; }
        
    }
}
