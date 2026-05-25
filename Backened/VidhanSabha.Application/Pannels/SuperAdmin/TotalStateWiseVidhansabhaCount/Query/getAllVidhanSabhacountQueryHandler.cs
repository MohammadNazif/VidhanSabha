using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Charts;
using MediatR;
using VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Interfaces;
using VidhanSabha.Domain.Enums;
using static VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Dtos.VidhanshabhaStateWiseCount;

namespace VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Query
{
    internal class getAllVidhanSabhacountQueryHandler : IRequestHandler<getAllvidhanSabhaCountQuery, IReadOnlyList<VidhansabhaResponseDto>>
    {
        private IStateWiseVidhanSabhaCountRepository _repo;

        public getAllVidhanSabhacountQueryHandler(IStateWiseVidhanSabhaCountRepository repo)
        {
            _repo = repo;   
        }
        public async Task<IReadOnlyList<VidhansabhaResponseDto>> Handle(getAllvidhanSabhaCountQuery request, CancellationToken cancellationToken)
        {
            if (IsUserRole(request.Role, PrabhariRole.SUPERADMIN))
            {
                request.UserId = null;
            }
            var res = await _repo.GetAllAsync(request.UserId);
            return res;
        }
        private bool IsUserRole(string currentRole, PrabhariRole roleToCheck)
        {
            return currentRole == roleToCheck.ToString();
        }
    }
} 
