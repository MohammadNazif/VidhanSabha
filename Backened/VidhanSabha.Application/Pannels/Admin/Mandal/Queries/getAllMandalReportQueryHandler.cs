using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs;
using VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Queries
{
    internal class getAllMandalReportQueryHandler : IRequestHandler<getAllMandalReportQuery, PagedResult<MandalReportDto>>
    {
        private IMandalRepository _mandal;

        public getAllMandalReportQueryHandler(IMandalRepository mandal)
        {
            _mandal = mandal;
        }
        public async Task<PagedResult<MandalReportDto>> Handle(getAllMandalReportQuery request, CancellationToken cancellationToken)
        {
            int? vidhanId = await   _mandal.GetVidhansabhaIdByuserIdAsync(request.QP.UserId);
             var data  = await _mandal.GetAllMandalReports(request.QP, vidhanId,  cancellationToken);
             return data;
        }
    }
}
