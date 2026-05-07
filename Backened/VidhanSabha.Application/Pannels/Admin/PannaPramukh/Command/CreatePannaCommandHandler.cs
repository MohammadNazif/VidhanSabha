using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.ImageService;
using VidhanSabha.Application.Common.ImageService.Interface;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Application.Pannels.Admin.PannaPramukh.Command
{
    public class CreatePannaCommandHandler : IRequestHandler<CreatePannaCommand, int>
    {
        private IPannaPramukhRepository _repo;
        private readonly IImageService _imageService;
        private readonly IBoothRepository _booth;

        public CreatePannaCommandHandler(IPannaPramukhRepository repo,IImageService imageService,IBoothRepository booth)
        {
            _repo = repo;
            _imageService = imageService;
            _booth = booth;
        }
        public async Task<int> Handle(CreatePannaCommand request, CancellationToken cancellationToken)
        {

            var imagePath = await request.Dto.ResolveImageAsync(
            _imageService,
            subFolder: "profiles/Panna",
            imageSelector: dto => dto.ProfilePicture
        );

            string createdtouserId = null;
            string createdsectorUserId = null;
            if (IsUserRole(request.Role, PrabhariRole.VidhanSabhaPrabhari))
            {
                createdsectorUserId = await _booth.GetSectorUseridbyBoothId(request.Dto.BoothId);
                createdtouserId = await _booth.GetUseridbyBoothId(request.Dto.BoothId);
            }
            else if (IsUserRole(request.Role, PrabhariRole.BoothSanyojak))
            {
                createdtouserId = request.UserId;
                createdsectorUserId = await _booth.GetSectorUseridbyBoothId(request.Dto.BoothId);
                request.UserId = await _booth.GetadminUseridbyUserId(request.Dto.BoothId);

            }
            else if (IsUserRole(request.Role, PrabhariRole.SectorSanyojak))
            {
                createdtouserId = await _booth.GetUseridbyBoothId(request.Dto.BoothId);

                createdsectorUserId = request.UserId;

                request.UserId = await _booth.GetadminUseridbyUserId(request.Dto.BoothId);

            }


            var pannapramukh = Tbl_PannaPramukh.Create(
                  request.Dto.BoothId, request.Dto.PannaNumber,
                  request.Dto.PannaPramukhName, request.Dto.CategoryId,
                  request.Dto.CastId, request.Dto.VoterId,
                  request.Dto.PhoneNumber, request.Dto.Address, request.UserId, createdtouserId,createdsectorUserId,request.Role,
                  request.Dto.VillageId, imagePath);

            _repo.AddAsync(pannapramukh, cancellationToken);

            return await Task.FromResult(pannapramukh.Id);

        }
        private bool IsUserRole(string currentRole, PrabhariRole roleToCheck)
        {
            return currentRole == roleToCheck.ToString();
        }
    }
}
