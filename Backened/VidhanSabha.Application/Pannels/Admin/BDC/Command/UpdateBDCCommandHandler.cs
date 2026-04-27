using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.ImageService.Interface;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.BDC.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Sector.Interface;

namespace VidhanSabha.Application.Pannels.Admin.BDC.Command
{
    public class UpdateBDCCommandHandler : IRequestHandler<UpdateBDCCommand, int>
    {
        private readonly IImageService _imageService;
        private IBDCRepository _repo;

        public UpdateBDCCommandHandler(IBDCRepository repo,IImageService imageService)
        {
            _imageService = imageService;
            _repo = repo;
        }
        public async Task<int> Handle(UpdateBDCCommand request, CancellationToken cancellationtoken)
        {
            var dto = request.Dto;

            var bdc = await _repo.GetByIdAsync(dto.Id)
                ?? throw new KeyNotFoundException($"BDC with Id {dto.Id} not found.");

            string? newImagePath = null;
            if (dto.Profile != null)
            {
                //if (!_imageService.IsValidImage(request.ProfileImage))
                //    throw new ValidationException("Invalid image. Only JPG/PNG/WEBP under 5MB allowed.");

                newImagePath = await _imageService.SaveImageAsync(
                    dto.Profile,
                    subFolder: "profiles/BDC"
                );

                // Delete old image only after new one saved successfully
                await _imageService.DeleteImageAsync(bdc.Profile);
            }

            if (bdc == null)
            {
                throw new NotFoundException("BDC Not Found");
            }
            bdc.Update(dto.Block,
                dto.Name,dto.WardNumber,dto.CategoryId,dto.CastId,dto.Age,dto.Mobile,
                dto.PartyId,dto.Education,newImagePath,dto.VillageId);
            return _repo.Update(bdc);
        }
    }
}
