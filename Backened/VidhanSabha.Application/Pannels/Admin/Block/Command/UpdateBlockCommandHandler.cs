using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.ImageService.Interface;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Block.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Application.Pannels.Admin.Sector.Interface;

namespace VidhanSabha.Application.Pannels.Admin.Block.Command
{
    public class UpdateBlockCommandHandler : IRequestHandler<UpdateBlockCommand, int>
    {
        private readonly IImageService _imageService;
        private IBlockRepository _repo;

        public UpdateBlockCommandHandler(IBlockRepository repo,IImageService imageService)
        {
            _imageService = imageService;
            _repo = repo;
        }
        public async Task<int> Handle(UpdateBlockCommand request, CancellationToken cancellationtoken)
        {
            var dto = request.Dto;

            var block = await _repo.GetByIdAsync(dto.Id)
                ?? throw new KeyNotFoundException($"Sector with Id {dto.Id} not found.");
            string? newImagePath = null;
            if (dto.Profile != null)
            {
                //if (!_imageService.IsValidImage(request.ProfileImage))
                //    throw new ValidationException("Invalid image. Only JPG/PNG/WEBP under 5MB allowed.");

                newImagePath = await _imageService.SaveImageAsync(
                    dto.Profile,
                    subFolder: "profiles/block"
                );

                // Delete old image only after new one saved successfully
                await _imageService.DeleteImageAsync(block.Profile);
            }
            if (block == null)
            {
                throw new NotFoundException("Block Not Found");
            }
            block.Update(dto.BlockName,
                dto.BlockPramukh,dto.PartyId, dto.Mobile,dto.Address, dto.CategoryId,
                dto.CastId, dto.OccupationId,newImagePath);
            return _repo.Update(block);
        }
    }
}
