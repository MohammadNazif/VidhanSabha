using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Dtos;

namespace VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Query
{
    public class getAllVidhanSabhaPrabhariQuery : IRequest<IReadOnlyList<StatePrabhariResponseDto>>
    {
        public int StateId { get; set; }

        public string UserId { get; set; }
        public getAllVidhanSabhaPrabhariQuery(int stateId,string userId)
        {
            StateId = stateId;
            UserId = userId;
        }

    }
}
