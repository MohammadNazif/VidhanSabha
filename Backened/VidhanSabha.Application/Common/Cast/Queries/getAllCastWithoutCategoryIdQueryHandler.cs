using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Cast.DTOs;
using VidhanSabha.Application.Common.Cast.Interfaces;

namespace VidhanSabha.Application.Common.Cast.Queries
{
    internal class getAllCastWithoutCategoryIdQueryHandler : IRequestHandler<getAllCastWithoutCategoryIdQuery, List<CastResponseDto>>
    {
        private ICastRepository _repo;

        public getAllCastWithoutCategoryIdQueryHandler(ICastRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<CastResponseDto>> Handle(getAllCastWithoutCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var casts = await _repo.GetAllCastAsync();

            var castResponse = casts.Select(c => new CastResponseDto
            {
                Id = c.Id,
                Name = c.CastName,
                Status = c.Status
            }).ToList();
            return castResponse;
        }
    }
}