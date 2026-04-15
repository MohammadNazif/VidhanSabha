using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Interfaces;
using VidhanSabha.Domain.Entities.SuperAdmin;

namespace VidhanSabha.Application.Pannels.SuperAdmin.TotalStateWiseVidhansabhaCount.Command
{
    internal class CreateVidhansabhaCountCommandHandler : IRequestHandler<CreateVidhansabhaCountCommand, int>
    {
        private IStateWiseVidhanSabhaCountRepository _repo;

        public CreateVidhansabhaCountCommandHandler(IStateWiseVidhanSabhaCountRepository repo)
        {
            _repo = repo;   
        }
        public async  Task<int> Handle(CreateVidhansabhaCountCommand request, CancellationToken cancellationToken)
        {
            var data =   Tbl_VidhansabhaStatewiseCount.Create(request.Dto.StateId, request.Dto.VidhanSabhaCount);
             var res = await _repo.AddAsync(data,cancellationToken);
            return res;
        }
    }
}
