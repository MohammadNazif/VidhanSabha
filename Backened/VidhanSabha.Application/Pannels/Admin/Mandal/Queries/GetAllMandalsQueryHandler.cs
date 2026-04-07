using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs.Create;
using VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Queries
{
    public class GetAllMandalsQueryHandler
        : IRequestHandler<GetAllMandalsQuery, List<MandalResponseDto>>
    {
        private readonly IMandalRepository _repo;

        public GetAllMandalsQueryHandler(IMandalRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<MandalResponseDto>> Handle(
            GetAllMandalsQuery query, CancellationToken ct)
        {
            var mandals = await _repo.GetAllAsync();
            if (mandals == null || !mandals.Any())
                throw new NotFoundException("No Mandals found.");

            return mandals.Select(m => new MandalResponseDto
            {
                Id = m.Id,
                VidhanId = m.VidhanId,
                Name = m.Name,
                Status = m.Status
            }).ToList();
        }
    }
}
