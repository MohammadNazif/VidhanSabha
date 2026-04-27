using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.ImageService;
using VidhanSabha.Application.Common.ImageService.Interface;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Sector.Interface;

namespace VidhanSabha.Application.Pannels.Admin.PannaPramukh.Command
{
    internal class UpdatePannaCommandHandler : IRequestHandler<UpdatePannaCommand, int>
    {
        private readonly IImageService _imageService;
        private IPannaPramukhRepository _repo;

        public UpdatePannaCommandHandler(IPannaPramukhRepository repo,IImageService imageService)
        {
            _imageService = imageService;
            _repo = repo;
        }
        public async Task<int> Handle(UpdatePannaCommand request, CancellationToken cancellationToken)
        {
            

            var dto = request.Dto;

            var panna = await _repo.GetByIdAsync(dto.Id)
                ?? throw new KeyNotFoundException($"PannaPramukh with Id {dto.Id} not found.");

            string? newImagePath = null;
            if (dto.ProfilePicture != null)
            {
                //if (!_imageService.IsValidImage(request.ProfileImage))
                //    throw new ValidationException("Invalid image. Only JPG/PNG/WEBP under 5MB allowed.");

                newImagePath = await _imageService.SaveImageAsync(
                    dto.ProfilePicture,
                    subFolder: "profiles/panna"
                );

                // Delete old image only after new one saved successfully
                await _imageService.DeleteImageAsync(panna.ProfilePicturePath);
            }
            if (panna == null)
            {
                throw new NotFoundException("Panna Pramukh Not Found");
            }

            panna.Update
                (dto.BoothId, dto.PannaNumber, dto.PannaPramukhName, dto.CategoryId,
                dto.CastId, dto.VoterId, dto.PhoneNumber, dto.Address, dto.VillageId,newImagePath);

             return _repo.Update(panna);

            
        }
    }
}
