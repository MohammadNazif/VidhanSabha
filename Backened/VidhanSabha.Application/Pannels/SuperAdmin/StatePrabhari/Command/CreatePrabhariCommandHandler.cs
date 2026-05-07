using DocumentFormat.OpenXml.Drawing.Charts;
using MediatR;
using VidhanSabha.Application.Common.CredentialMananger;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Interfaces;
using VidhanSabha.Domain.Entities.SuperAdmin;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Command
{
    public class CreatePrabhariCommandHandler : IRequestHandler<CreatePrabhariCommand, int>
    {
        private readonly IStatePrabhariRepository _repo;
        private readonly CredentialManagerFunc _credentialManager;
        // ✅ IUnitOfWork REMOVED — caller owns the transaction

        public CreatePrabhariCommandHandler(
            IStatePrabhariRepository repo,
            CredentialManagerFunc credentialManager)
        {
            _repo = repo;
            _credentialManager = credentialManager;
        }

        public async Task<int> Handle(CreatePrabhariCommand req, CancellationToken cancellationToken)
        {
            var request = req.Dto;
            var userId = $"USR_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
            if (IsUserRole(req.Role, PrabhariRole.SUPERADMIN))
            {
                request.PrabhariRole = PrabhariRole.StatePrabhari;
            }

            await _credentialManager.InsertCredentialAsync(
                userId: userId,
                mobile: request.ContactNumber,
                email: request.PrabhariEmail,
                role: request.PrabhariRole
            );

            // Step 2 — Insert tbl_stateprabhari
            var data = Tbl_StatePrabhari.Create(
                req.UserId,
                userId,
                request.stateId,
                request.vidhanSanhaId,
                request.PrabhariRole,          // ✅ BUG FIXED: was hardcoded to PrabhariRole.StatePrabhari
                request.PrabhariName,
                request.PrabhariEmail,
                request.Gender,
                request.ContactNumber,
                request.CategoryId,
                request.CastId,
                request.Education,
                request.Profession,
                request.CurrentAddress
            );

            return await _repo.AddAsync(data);
        }
        private bool IsUserRole(string currentRole, PrabhariRole roleToCheck)
        {
            return currentRole == roleToCheck.ToString();
        }
    }
}