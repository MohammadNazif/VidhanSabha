using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs;
using VidhanSabha.Application.Pannels.Admin.Mandal.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Queries
{
    internal class getMandalSanyojakQueryHandler : IRequestHandler<getMandalSanyojakQuery, MandalSanyojakDto>
    {
        private IMandalRepository _repo;

        public getMandalSanyojakQueryHandler(IMandalRepository repo)
        {
            _repo = repo;   
        }
        public Task<MandalSanyojakDto> Handle(getMandalSanyojakQuery request, CancellationToken cancellationToken)
        {
            return _repo.GetMandalSanyojakByIdAsync(request.Id);
           
        }
    }
}
