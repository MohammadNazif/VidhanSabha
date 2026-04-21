using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.SahmatAsahmatType.DTOs;
using VidhanSabha.Application.Common.SeniorDisabledType.DTOs;

namespace VidhanSabha.Application.Common.SeniorDisabledType.Queries
{
    public class GetAllSeniorDisabledTypeQuery : IRequest<List<SeniorDisabledTypeResponseDto>>
    {
    }
}
