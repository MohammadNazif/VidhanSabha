using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Sector.DTOs;
using VidhanSabha.Application.Pannels.Admin.Sector.Interface;

namespace VidhanSabha.Application.Pannels.Admin.Sector.Queries
{
    public class GetAllSectorsHandler : IRequestHandler<GetAllSectorsQuery, PagedResult<SectorResponseDto>>
    {
        private readonly ISectorRepository _repo;
        private readonly IMandalRepository _man;

        public GetAllSectorsHandler(ISectorRepository repo,IMandalRepository man)
        {
            _repo = repo;
            _man = man;
        }

        public async Task<PagedResult<SectorResponseDto>> Handle(GetAllSectorsQuery request, CancellationToken cancellationToken)
        {
            int? vidhanId = await _man.GetVidhansabhaIdByuserIdAsync(request.UserId);
            if (vidhanId == 0)
            {
                throw new NotFoundException("Vidhansabha Not Found");
            }
            var sectors = await _repo.GetAllAsync(request.QueryParams,vidhanId, cancellationToken);
            if(sectors==null)
            {
                throw new NotFoundException("Sector Not Found");
            }

            return sectors;
            
        }
    }
}
