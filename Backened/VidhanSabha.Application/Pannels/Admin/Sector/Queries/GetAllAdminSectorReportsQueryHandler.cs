using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;
using VidhanSabha.Application.Pannels.Admin.Sector.Interface;

namespace VidhanSabha.Application.Pannels.Admin.Sector.Queries
{
    public class GetAllAdminSectorReportsQueryHandler : IRequestHandler<GetAllAdminSectorReportsQuery, PagedResult<AdminSectorReportsDto>>
    {
        private ISectorRepository _repo;

        public GetAllAdminSectorReportsQueryHandler(ISectorRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<AdminSectorReportsDto>> Handle(GetAllAdminSectorReportsQuery request, CancellationToken cancellationToken)
        {
            var sectors = await _repo.GetAllAdminSectorReports(request.QueryParams, cancellationToken);
            if (sectors == null)
            {
                throw new NotFoundException("Sector Not Found");
            }

            return sectors;
        }
    }
}
