using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.District.Interfaces;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.StatePrabhari.DistrictWiseCount.Interface;
using static VidhanSabha.Application.Pannels.StatePrabhari.DistrictWiseCount.Dtos.DistrictWiseCount;

namespace VidhanSabha.Domain.Entities.StatePrabhari.DistrictWiseCount.Query
{
    internal class getAllDitrictWiseCountQueryHandler : IRequestHandler<getAllDitrictWiseCountQuery, IReadOnlyList<VidhansabhaDistrictResponseDto>>
    {
        private IDistrictWiseCount _repo;

        public getAllDitrictWiseCountQueryHandler(IDistrictWiseCount repo)
        {
            _repo = repo;
        }
        public Task<IReadOnlyList<VidhansabhaDistrictResponseDto>> Handle(getAllDitrictWiseCountQuery request, CancellationToken cancellationToken)
        {
             var data =   _repo.GetByIdAsync(request.UserId);

            if(data == null)
            {
                throw new NotFoundException("Data Not Found");
            }
            return data;
        }
    }
}
