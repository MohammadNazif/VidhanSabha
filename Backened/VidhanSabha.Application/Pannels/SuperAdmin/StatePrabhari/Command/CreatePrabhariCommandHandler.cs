using MediatR;
using VidhanSabha.Application.Common.CredentialMananger;
using VidhanSabha.Application.Common.UnitOfWork;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Interfaces;
using VidhanSabha.Domain.Entities.SuperAdmin;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Command
{
    public class CreatePrabhariCommandHandler : IRequestHandler<CreatePrabhariCommand, int>
    {
        private readonly IStatePrabhariRepository _repo;
        private readonly CredentialManagerFunc _credentialManager;
        private readonly IUnitOfWork _uow;

        public CreatePrabhariCommandHandler(
            IStatePrabhariRepository repo,
            CredentialManagerFunc credentialManager,
            IUnitOfWork uow)
        {
            _repo = repo;
            _credentialManager = credentialManager;
            _uow = uow;
        }

        public async Task<int> Handle(CreatePrabhariCommand req, CancellationToken cancellationToken)
        {
            var request = req.Dto;
            var userId = $"USR_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
            await _uow.BeginTransactionAsync();

            try
            {
                // Step 1 — Insert tbl_login first
                await _credentialManager.InsertCredentialAsync(
                     userId: userId,
                    mobile: request.ContactNumber,
                    email: request.PrabhariEmail,
                    role: "StatePrabhari"
                );

                // Step 2 — Insert tbl_stateprabhari
                var data = Tbl_StatePrabhari.Create(
                    userId,
                    request.stateId,
                    request.vidhanSanhaId,
                    request.PrabhariRole,
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

                var result = await _repo.AddAsync(data);

                // Step 3 — Both success → commit
                await _uow.CommitAsync();

                return result;
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                throw new ApplicationException($"Prabhari registration failed. Rolled back. Reason: {ex.Message}", ex);
            }
        }
    }
}