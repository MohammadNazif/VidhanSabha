using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Queries
{
    public class getAllMandalReportQuery : IRequest<PagedResult<MandalReportDto>>
    {
        public MandalQueryParams QP { get; set; }
        public getAllMandalReportQuery(MandalQueryParams qp)
        {
            QP = qp;
        }
    }
}
