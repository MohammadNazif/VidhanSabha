using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.InkML;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.MandalSamiti.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.MandalSamiti.Command
{
    internal class CreateMandalSamitiMemberCommandHandler : IRequestHandler<CreateMandalSamitiMemberCommand, int>
    {
        private IMandalSamiti _repo;

        public CreateMandalSamitiMemberCommandHandler(IMandalSamiti repo)
        {
            _repo = repo;

        }
        public async Task<int> Handle(CreateMandalSamitiMemberCommand request, CancellationToken cancellationToken)
        {
            // 1️⃣ Fetch MandalSamiti record
            var mandalSamiti = await _repo.GetMandalSamitiByIdAsync(
                request.Dto.MemberId, cancellationToken);

            if (mandalSamiti == null)
                throw new Exception("MandalSamiti not found");

            var member = Tbl_MandalSamitiMem.Create(
                request.Dto.Name,
                request.Dto.Age,
                request.Dto.Contact,
                request.Dto.Occupation,
                request.Dto.DesignationId,
                request.Dto.CategoryId,
                request.Dto.CasteId,
                request.Dto.UserId,
                request.Dto.MandalId,
                null
            );

            // 3️⃣ Increment counter
              mandalSamiti.Increment();

            // 4️⃣ Stage both changes (no DB hit yet)
            _repo.UpdateMandalSamiti(mandalSamiti);
            _repo.InsertMandalSamitiMember(member);

            // 5️⃣ ✅ Single SaveChanges — both committed in one transaction
            return await _repo.SaveChangesAsync(cancellationToken);
        }
    }
}
