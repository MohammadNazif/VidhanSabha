using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.DesignatinType.Dto;

namespace VidhanSabha.Application.Common.DesignatinType.Query
{
    public class getdesignationTypeQuery : IRequest<List<DesignationTypeResponseDto>>
    {

    }
}
