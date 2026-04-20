using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.AdminDesignation.DTOs;
using VidhanSabha.Application.Common.AdminDesignation.Interfaces;
using VidhanSabha.Application.Common.AdminDesignation.Query;
using VidhanSabha.Application.Common.Party.DTOs;
using VidhanSabha.Application.Common.Party.Interfaces;
using VidhanSabha.Application.Common.Party.Queries;
using VidhanSabha.Application.Exceptions;

namespace VidhanSabha.Application.Common.AdminDesignation.Queries
{
    public class GetAllAdminDesignationQueryHandler : IRequestHandler<GetAllAdminDesignationQuery, List<AdminDesignationResponseDto>>
    {
        private IAdminDesignationRepository _repo;

        public GetAllAdminDesignationQueryHandler(IAdminDesignationRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<AdminDesignationResponseDto>> Handle(GetAllAdminDesignationQuery query, CancellationToken cancellationtoken)
        {
            var res = await _repo.GetAllAsync();
            if (res == null)
            {
                throw new NotFoundException("Party Not Found");
            }
            return res.Select(x => new AdminDesignationResponseDto
            {
                Id = x.Id,
                DesignationName = x.DesignationName,
            }).ToList();
        }
    }
}
