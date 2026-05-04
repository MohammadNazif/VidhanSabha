using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Queries
{
    internal class getboothbysectoridqueryHandler : IRequestHandler<getboothbysectoridquery, List<BoothNumberDto>>
    {
        private IBoothRepository _booth;

        public getboothbysectoridqueryHandler(IBoothRepository booth)
        {
            _booth = booth;
        }
        public async Task<List<BoothNumberDto>> Handle(getboothbysectoridquery request, CancellationToken cancellationToken)
        {
              return  await _booth.BoothBysectorId(request.UserId);
        }
    }
}
