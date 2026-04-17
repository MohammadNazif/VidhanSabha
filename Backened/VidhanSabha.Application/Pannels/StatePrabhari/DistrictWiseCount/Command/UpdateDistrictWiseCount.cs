using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace VidhanSabha.Domain.Entities.StatePrabhari.DistrictWiseCount.Command
{
    internal class UpdateDistrictWiseCount : IRequest<int>
    {

        public UpdateDistrictWiseCount Dto;
        public UpdateDistrictWiseCount()
        {
            
        }
    }
}
