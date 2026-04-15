using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.DesignatinType.Dto;
using VidhanSabha.Application.Common.DesignatinType.Interface;

namespace VidhanSabha.Application.Common.DesignatinType.Query
{
    public class getdesignationTypeQueryHandler : IRequestHandler<getdesignationTypeQuery, List<DesignationTypeResponseDto>>
    {
        private IDesignationType _repo;

        public getdesignationTypeQueryHandler(IDesignationType repo)
        {
            _repo = repo;
        }
        public Task<List<DesignationTypeResponseDto>> Handle(getdesignationTypeQuery request, CancellationToken cancellationToken)
        {
           return _repo.getAllAsync(cancellationToken);
        }
    }
}
