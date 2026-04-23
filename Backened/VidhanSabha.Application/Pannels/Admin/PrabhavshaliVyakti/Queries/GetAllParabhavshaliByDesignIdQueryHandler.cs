using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.DTOs;
using VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Queries
{
    public class GetAllParabhavshaliByDesignIdQueryHandler:IRequestHandler<GetAllParabhavshaliByDesignIdQuery, List<PrabhavshaliResponseDesinIdDto>>
    {
        private IPrabhavshaliRepository _repo;

        public GetAllParabhavshaliByDesignIdQueryHandler(IPrabhavshaliRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<PrabhavshaliResponseDesinIdDto>> Handle(GetAllParabhavshaliByDesignIdQuery request, CancellationToken cancellationToken)
        {
            var res = await _repo.GetByDesgIdAsync(request.DesgId);

            if (res == null )
            {
                throw new NotFoundException("Prabhavshali Vyakti Not Found");
            }

            return res;
        }
    }
}
