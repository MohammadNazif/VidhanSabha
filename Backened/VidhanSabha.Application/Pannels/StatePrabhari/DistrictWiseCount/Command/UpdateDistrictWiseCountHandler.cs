using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.StatePrabhari.DistrictWiseCount.Interface;
using VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Interfaces;

namespace VidhanSabha.Domain.Entities.StatePrabhari.DistrictWiseCount.Command
{
    internal class UpdateDistrictWiseCountHandler : IRequestHandler<UpdateDistrictWiseCount, int>
    {
        private IDistrictWiseCount _repo;
        private IStateWiseVidhanSabhaCountRepository _count;

        public UpdateDistrictWiseCountHandler(IDistrictWiseCount repo,IStateWiseVidhanSabhaCountRepository count)
        {
            _repo = repo;
            _count = count;
        }
        public async  Task<int> Handle(UpdateDistrictWiseCount request, CancellationToken cancellationToken)
        {

            var req = request.Dto;
            var data = await _count.GetByIdAsync(req.StateId);
            if (data.Remainingcount < req.VidhanSabhaCount)
            {
                throw new Exception("Vidhansabha Count should be less than or equal to remaining count");
            }
            var disdata = await _repo.GetByDistrictIdAsync(request.Dto.DistrictId,req.UserId, cancellationToken);

              data.UpdateCount(disdata.VidhansabhaCount, req.VidhanSabhaCount, data.Remainingcount);
             disdata.Update(req.DistrictId, req.VidhanSabhaCount);
                _count.Update(data);

                _repo.Update(disdata);

            return await _repo.SaveChangesAsync(cancellationToken);
        }
    }
}
