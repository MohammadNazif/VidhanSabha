using MediatR;
using VidhanSabha.Application.Common.UnitOfWork;
using VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Command;
using VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Interface;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Command;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Dtos;
using VidhanSabha.Domain.Entities.StatePrabhari;
using VidhanSabha.Domain.Enums;

public class CreateVidhanSabhaCommandHandler : IRequestHandler<CreateVidhanSabhaCommand, int>
{
    private readonly IVidhanSabhaRepository _vidhanSabhaRepo;
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _uow;

    public CreateVidhanSabhaCommandHandler(
        IVidhanSabhaRepository vidhanSabhaRepo,
        IMediator mediator,
        IUnitOfWork uow)
    {
        _vidhanSabhaRepo = vidhanSabhaRepo;
        _mediator = mediator;
        _uow = uow;
    }

    public async Task<int> Handle(CreateVidhanSabhaCommand request, CancellationToken cancellationToken)
    {
        var req = request.Dto;
        int vidhanSabhaId = 0;

        await _uow.BeginTransactionAsync();
        try
        {
            // Step 1 — Check if VidhanSabha already exists
            var existing = await _vidhanSabhaRepo.GetByVidhanIdAsync(req.VidhanSabhaId);
            if (existing == null)
            {
                var vidhanSabha = Tbl_VidhanSabha.Create(
                    req.VidhanSabhaName,
                    req.vidhanSabhaNumber,
                    req.DistrictId,
                    req.UserId,
                    req.stateId
                );
                vidhanSabhaId = await _vidhanSabhaRepo.AddAsync(vidhanSabha);
            }
            else
            {
                vidhanSabhaId = existing.vidhanSabhaId;
            }

            // Step 2 — Optionally create Prabhari (no nested transaction — outer tx covers it)
            if (req.Prabhari != null && req.isPrabhari)
            {
                var p = req.Prabhari;
                await _mediator.Send(
                    new CreatePrabhariCommand(
                        new CreatePrabhariRequestDto
                        {
                            CreatedByUserId = request.UserId,
                            PrabhariRole = PrabhariRole.VidhanSabhaPrabhari,
                            stateId = p.stateId,
                            vidhanSanhaId = vidhanSabhaId,
                            PrabhariName = p.PrabhariName,
                            PrabhariEmail = p.PrabhariEmail,
                            Gender = p.Gender,
                            ContactNumber = p.ContactNumber,
                            CategoryId = p.CategoryId,
                            CastId = p.CastId,
                            Education = p.Education,
                            Profession = p.Profession,
                            CurrentAddress = p.CurrentAddress
                        },
                        request.UserId,
                        request.Role
                    ),
                    cancellationToken
                );
            }

            // Step 3 — Everything succeeded, commit the single transaction
            await _uow.CommitAsync();
            return vidhanSabhaId;
        }
        catch (Exception ex)
        {
            await _uow.RollbackAsync();
            throw new ApplicationException(
                $"VidhanSabha creation failed. Rolled back. Reason: {ex.Message}", ex);
        }
    }
}