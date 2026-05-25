using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Common.UnitOfWork;
using VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Interface;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Command;
using VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Dtos;
using VidhanSabha.Domain.Entities.StatePrabhari;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Application.Pannels.StatePrabhari.VidhanSabha.Command
{
    internal class UpdateVidhanSabhaCommandHandler : IRequestHandler<UpdateVidhanSabhaCommand, int>
    {
        private readonly IVidhanSabhaRepository _vidhanSabhaRepo;
        private readonly IMediator _mediator;              // ✅ MediatR injected
        private readonly IUnitOfWork _uow;
        public UpdateVidhanSabhaCommandHandler(IVidhanSabhaRepository vidhanSabhaRepo,
        IMediator mediator,
        IUnitOfWork uow)
        {
            _vidhanSabhaRepo = vidhanSabhaRepo;
            _mediator = mediator;
            _uow = uow;
        }
        public async Task<int> Handle(UpdateVidhanSabhaCommand req, CancellationToken cancellationToken)
        {

            var request = req.Dto;
            await _mediator.Send(new UpdatePrabhariCommand(new UpdatePrabhariRequestDto
            {
                Id = request.Id,
                PrabhariName = request.PrabhariName,
                PrabhariEmail = request.PrabhariEmail,
                Gender = request.Gender,
                ContactNumber = request.ContactNumber,
                CategoryId = request.CategoryId,
                CastId = request.CastId,
                Education = request.Education,
                Profession = request.Profession,
                CurrentAddress = request.CurrentAddress
            },req.UserId), cancellationToken);

            return 1;
        }

    }
}