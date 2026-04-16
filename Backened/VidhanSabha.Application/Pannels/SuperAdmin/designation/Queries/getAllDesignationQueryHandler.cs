using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.SuperAdmin.designation.DTOs;
using VidhanSabha.Application.Pannels.SuperAdmin.designation.Interfaces;

namespace VidhanSabha.Application.Pannels.SuperAdmin.designation.Queries
{
    internal class getAllDesignationQueryHandler : IRequestHandler<getAllDesignationQuery, IReadOnlyList<DesignationResponseDto>>
    {
        private IDesignationRepository _repo;

        public getAllDesignationQueryHandler(IDesignationRepository repo)
        {
            _repo = repo;
        }
      
        Task<IReadOnlyList<DesignationResponseDto>> IRequestHandler<getAllDesignationQuery, IReadOnlyList<DesignationResponseDto>>.Handle(getAllDesignationQuery request, CancellationToken cancellationToken)
        {
            return _repo.GetAllAsync(request.UserId);
        }
    }
}
