using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;
using VidhanSabha.Application.Pannels.Admin.Sector.Interface;

namespace VidhanSabha.Application.Pannels.Admin.Sector.Queries
{
    public class GetAllSectorReportsQueryHandler : IRequestHandler<GetAllSectorReportsQuery, PagedResult<SectorReportDto>>
    {
        private ISectorRepository _repo;
        private IMandalRepository _mandal;

        public GetAllSectorReportsQueryHandler(ISectorRepository repo, IMandalRepository mandal)
        {
            _repo = repo;
            _mandal = mandal;
        }
        public async Task<PagedResult<SectorReportDto>> Handle(GetAllSectorReportsQuery request, CancellationToken cancellationToken)
        {
            int? VidhanSabhaId = await _mandal.GetVidhansabhaIdByuserIdAsync(request.QueryParams.UserId);
            var sectors = await _repo.GetAllSectorReports(request.QueryParams, VidhanSabhaId ,cancellationToken);
            if (sectors == null)
            {
                throw new NotFoundException("Sector Not Found");
            }

            return sectors;
        }
    }
}
