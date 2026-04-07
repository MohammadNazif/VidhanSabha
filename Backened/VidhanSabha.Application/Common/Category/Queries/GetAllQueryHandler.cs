using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Category.DTOs;
using VidhanSabha.Application.Common.Category.Interfaces;

namespace VidhanSabha.Application.Common.Category.Queries
{
    public class GetAllCatgeoryQueryHandler : IRequestHandler<GetallCatgeory, List<CategoryResponseDto>>
    {
        private ICategoryRepository _repo;

        public GetAllCatgeoryQueryHandler(ICategoryRepository  repo)
        {
            _repo = repo;
        }
        public async Task<List<CategoryResponseDto>> Handle(GetallCatgeory request, CancellationToken cancellationToken)
        {
            var categories =  await _repo.GetAllAsync();
            return categories.Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Status = c.Status
            }).ToList();
        }
    }
}
