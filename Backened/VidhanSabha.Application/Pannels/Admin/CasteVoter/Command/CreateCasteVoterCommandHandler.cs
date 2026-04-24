using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.CasteVoter.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.ValueObjects;

namespace VidhanSabha.Application.Pannels.Admin.CasteVoter.Command
{
    public class CreateCasteVoterCommandHandler : IRequestHandler<CreateCasteVoterCommand, int>
    {
        private readonly ICasteVoterRepository _repo;

        public CreateCasteVoterCommandHandler(ICasteVoterRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(CreateCasteVoterCommand request, CancellationToken cancellationToken)
        {
            var req = request.Dto;

            if (req.CasteVoters == null || !req.CasteVoters.Any())
                throw new Exception("Caste voter data is required.");

            if (req.CasteVoters.GroupBy(x => x.SubCasteId).Any(g => g.Count() > 1))
                throw new Exception("Duplicate SubCasteId not allowed.");

            if (req.CasteVoters.Any(x => x.Number <= 0))
                throw new Exception("Number must be greater than 0.");

            var totalVoter = await _repo.GetTotalVoterByCasteVoterIdAsync(
                req.CasteVoterId,
                cancellationToken
            );

            if (totalVoter == 0)
                throw new Exception("Booth voter data not found.");

            // 🔥 FIX (existing data include karo)
            var existing = await _repo.GetByCasteVoterIdAsync(
                req.CasteVoterId,
                cancellationToken
            );

            var existingSum = existing.Sum(x => x.Number);

            var newSum = req.CasteVoters.Sum(x => x.Number);

            if (existingSum + newSum > totalVoter)
                throw new Exception("Total caste voters cannot exceed total voters.");

            var entities = req.CasteVoters
                .Select(x => Tbl_CasteVoter.Create(
                    req.CasteVoterId,
                    x.SubCasteId,
                    x.Number
                ))
                .ToList();

            await _repo.AddRangeAsync(entities, cancellationToken);

            return entities.Count;
        }
    }
}
