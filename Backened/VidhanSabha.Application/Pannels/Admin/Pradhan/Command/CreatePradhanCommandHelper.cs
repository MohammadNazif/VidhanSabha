using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.Pradhan.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Pradhan.Command
{
    public class CreatePradhanCommandHelper: IRequestHandler<CreatePradhanCommand, int>
    {
        private readonly IPradhanRepository _repo;
        public CreatePradhanCommandHelper(IPradhanRepository repo)
        {
            _repo = repo;
        }                                         

        public async Task<int> Handle(CreatePradhanCommand request, CancellationToken cancellationToken)
        {
            var req = request.Dto;
            if (!Enum.IsDefined(typeof(VidhanSabha.Domain.Enums.Gender), req.Gender))
            {
                throw new Exception("Invalid Gender Value");
            }
            

            var data = Tbl_Pradhan.Create(
                req.Name, req.DesignationId, req.Contact, req.Gender, req.VillageId);

            return await _repo.AddAsync(data, cancellationToken);
        }
    }
}
