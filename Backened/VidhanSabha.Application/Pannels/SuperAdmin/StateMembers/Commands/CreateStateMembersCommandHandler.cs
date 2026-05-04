using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.ImageService;
using VidhanSabha.Application.Common.ImageService.Interface;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Entities.SuperAdmin;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.Commands
{
    public class CreateStateMembersCommandHandler : IRequestHandler<CreateStateMembersCommand, int>
    {
        private readonly IImageService _imageService;
        private readonly IStateMembersRepository _repo;
        public CreateStateMembersCommandHandler(IStateMembersRepository repo,IImageService imageService)
        {
            _imageService = imageService;
            _repo = repo;
        }
        public async Task<int> Handle(CreateStateMembersCommand request, CancellationToken cancellationToken)
        {

            var imagePath = await request.Dto.ResolveImageAsync(
            _imageService,
            subFolder: "profiles/StateMember",
            imageSelector: dto => dto.Profile
        );
            var dto = request.Dto;

            var members = Tbl_StateMembers.Create(
                dto.DesignationId,dto.DesignationTypeId,dto.Name,dto.Email,dto.Mobile,request.UserId ,dto.CategoryId,
                dto.CastId, imagePath,dto.Education,dto.DOB,dto.Address,dto.Proffesion
               );

            _repo.AddAsync(members, cancellationToken);

            return await Task.FromResult(members.Id);

        }

    }
}
