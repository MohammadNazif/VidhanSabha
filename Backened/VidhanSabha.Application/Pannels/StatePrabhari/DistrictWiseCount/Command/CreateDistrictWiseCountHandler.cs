using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.District.Interfaces;
using VidhanSabha.Application.Pannels.StatePrabhari.DistrictWiseCount.Interface;
using VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Interfaces;
using VidhanSabha.Domain.Entities.SuperAdmin;

namespace VidhanSabha.Domain.Entities.StatePrabhari.DistrictWiseCount.Command
{
    internal class CreateDistrictWiseCountHandler : IRequestHandler<CreateDistrictWiseCount, int>
    {
        private IStateWiseVidhanSabhaCountRepository _repo;
        private IDistrictWiseCount _dis;

        public CreateDistrictWiseCountHandler(IStateWiseVidhanSabhaCountRepository repo, IDistrictWiseCount dis)
        {
            _repo = repo;
            _dis = dis;
        }
        public  async Task<int> Handle(CreateDistrictWiseCount request, CancellationToken cancellationToken)
        {
    
             var req = request.Dto;
              var data = await  _repo.GetByIdAsync(req.StateId);

            if(data.Remainingcount < req.VidhanSabhaCount)
            {
                throw new Exception("Vidhansabha Count should be less than or equal to remaining count");
            }
         
            data.DecrementRemaining(data.VidhansabhaCount, req.VidhanSabhaCount);

            _repo.Update(data);

              var res =Tbl_DistrictWiseCount.Create(req.DistrictId, req.VidhanSabhaCount,req.UserId);
                return await _dis.AddAsync(res);
        }
    }
}
