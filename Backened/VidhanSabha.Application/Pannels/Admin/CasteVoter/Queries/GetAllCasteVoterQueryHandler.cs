using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.CasteVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.CasteVoter.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.CasteVoter.Queries
{
    public class GetAllCasteVoterQueryHandler: IRequestHandler<GetAllCasteVoterQuery, PagedResult<CasteVoterResponseDto>>
    {
        private readonly ICasteVoterRepository _repo;

        public GetAllCasteVoterQueryHandler(ICasteVoterRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<CasteVoterResponseDto>> Handle(
            GetAllCasteVoterQuery request,
            CancellationToken cancellationToken)
        {
            return await _repo.GetAllAsync(cancellationToken);
        }
    }
}
