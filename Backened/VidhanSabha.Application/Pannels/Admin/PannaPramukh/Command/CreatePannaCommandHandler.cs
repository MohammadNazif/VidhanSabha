using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.ImageService;
using VidhanSabha.Application.Common.ImageService.Interface;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.PannaPramukh.Command
{
    public class CreatePannaCommandHandler : IRequestHandler<CreatePannaCommand, int>
    {
        private IPannaPramukhRepository _repo;
        private readonly IImageService _imageService;

        public CreatePannaCommandHandler(IPannaPramukhRepository repo,IImageService imageService)
        {
            _repo = repo;
            _imageService = imageService;
        }
        public async Task<int> Handle(CreatePannaCommand request, CancellationToken cancellationToken)
        {

            var imagePath = await request.Dto.ResolveImageAsync(
            _imageService,
            subFolder: "profiles/Panna",
            imageSelector: dto => dto.ProfilePicture
        );

            var pannapramukh =  Tbl_PannaPramukh.Create(
              request.Dto.BoothId, request.Dto.PannaNumber, 
              request.Dto.PannaPramukhName, request.Dto.CategoryId, 
              request.Dto.CastId, request.Dto.VoterId, 
              request.Dto.PhoneNumber, request.Dto.Address, request.UserId,
              request.Dto.VillageId,imagePath);

             _repo.AddAsync(pannapramukh,cancellationToken);
            
            return await Task.FromResult(pannapramukh.Id);

        }
    }
}
