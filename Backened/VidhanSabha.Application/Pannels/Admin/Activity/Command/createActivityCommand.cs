using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Activity.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.Activity.Command
{
    public record createActivityCommand(CreateActivityDto Dto) : IRequest<int>;
}
