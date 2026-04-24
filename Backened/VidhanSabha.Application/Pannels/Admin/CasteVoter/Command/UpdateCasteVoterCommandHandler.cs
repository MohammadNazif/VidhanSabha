using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.CasteVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Command;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.CasteVoter.Command
{
    public class UpdateCasteVoterCommandHandler : IRequestHandler<UpdateCasteVoterCommand, int>
    {
        private ICasteVoterRepository _repo;
        public UpdateCasteVoterCommandHandler(ICasteVoterRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(UpdateCasteVoterCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            if (dto.CasteVoters == null || !dto.CasteVoters.Any())
                throw new Exception("Caste voter data is required.");

            if (dto.CasteVoters.GroupBy(x => x.SubCasteId).Any(g => g.Count() > 1))
                throw new Exception("Duplicate SubCasteId not allowed.");

            if (dto.CasteVoters.Any(x => x.Number <= 0))
                throw new Exception("Number must be greater than 0.");

            var existing = await _repo.GetByCasteVoterIdAsync(dto.CasteVoterId, cancellationToken);

            if (!existing.Any())
                throw new NotFoundException("Caste voter data not found for this Booth.");

            var totalVoter = await _repo.GetTotalVoterByCasteVoterIdAsync(dto.CasteVoterId, cancellationToken);

            if (totalVoter <= 0)
                throw new Exception("Booth voter data not found.");

            // ✔️ SAFE CHECK
            var newSum = dto.CasteVoters.Sum(x => x.Number);

            if (newSum > totalVoter)
                throw new Exception("Total SubCaste voters cannot exceed Total Voters.");

            // delete old
            await _repo.DeleteRangeAsync(existing, cancellationToken);

            // add new
            var newEntities = dto.CasteVoters
                .Select(x => Tbl_CasteVoter.Create(
                    dto.CasteVoterId,
                    x.SubCasteId,
                    x.Number
                ))
                .ToList();

            return await _repo.AddRangeAsync(newEntities, cancellationToken);
        }

    }
}
