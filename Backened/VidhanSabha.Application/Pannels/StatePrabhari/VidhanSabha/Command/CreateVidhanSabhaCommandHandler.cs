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
    private readonly IMediator _mediator;              // ✅ MediatR injected
    private readonly IUnitOfWork _uow;

    public CreateVidhanSabhaCommandHandler(
        IVidhanSabhaRepository vidhanSabhaRepo,
        IMediator mediator,                            // ✅ No need for _prabhariRepo
        IUnitOfWork uow)                               //    or _credentialManager here
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
          
              var data =  await  _vidhanSabhaRepo.GetByVidhanIdAsync(req.VidhanSabhaId);
               if(data == null)
              {
                var vidhanSabha = Tbl_VidhanSabha.Create(req.VidhanSabhaName, req.VidhanSabhaCount, req.DistrictId, req.UserId, req.stateId);

                 vidhanSabhaId = await _vidhanSabhaRepo.AddAsync(vidhanSabha);
               }
              else
             {
                vidhanSabhaId = data.vidhanSabhaId;
              }
             
          
            if (req.Prabhari != null && req.isPrabhari)
            {
                var p = req.Prabhari;

                
                await _mediator.Send(new CreatePrabhariCommand(new CreatePrabhariRequestDto
                {
                    PrabhariRole = PrabhariRole.VidhanSabha,
                    stateId  = p.stateId,
                    vidhanSanhaId = vidhanSabhaId, 
                    PrabhariName =p.PrabhariName,
                    PrabhariEmail = p.PrabhariEmail,
                    Gender = p.Gender,
                    ContactNumber = p.ContactNumber,
                    CategoryId = p.CategoryId,
                    CastId = p.CastId,
                    Education = p.Education,
                    Profession = p.Profession,
                    CurrentAddress = p.CurrentAddress
                }), cancellationToken);
            }

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