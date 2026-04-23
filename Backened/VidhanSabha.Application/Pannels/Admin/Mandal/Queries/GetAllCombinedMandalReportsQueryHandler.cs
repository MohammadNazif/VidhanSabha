using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs.Create;
using VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Queries
{
    public class GetAllCombinedMandalReportsQueryHandler
        : IRequestHandler<GetAllCombinedMandalReportsQuery, PagedResult<CombinedMandalReportDto>>
    {
        private readonly IMandalRepository _repo;

        public GetAllCombinedMandalReportsQueryHandler(IMandalRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<CombinedMandalReportDto>> Handle(GetAllCombinedMandalReportsQuery request, CancellationToken ct)
        {
            var mandals = await _repo.GetAllCombinedMandalReports(request.QueryParams,ct);

            if (mandals == null)
            {
                throw new NotFoundException("No Mandals found.");
            }
            return mandals;
        }
    }
}
