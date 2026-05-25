using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.ImageService;
using VidhanSabha.Application.Common.ImageService.Interface;
using VidhanSabha.Application.Pannels.Admin.Block.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Block.Command
{
    public class CreateBlockCommandHandler : IRequestHandler<CreateBlockCommand, int>
    {
        private readonly IImageService _imageService;
        private readonly IBlockRepository _repo;

        public CreateBlockCommandHandler(IBlockRepository repo,IImageService imageService)
        {
            _imageService=imageService;
            _repo = repo;
        }
        public async Task<int> Handle(CreateBlockCommand request, CancellationToken cancellationToken)
        {
            var imagePath = "";

             imagePath = await request.Dto.ResolveImageAsync(
            _imageService,
            subFolder: "profiles/Block",
            imageSelector: dto => dto.Profile
        );

            var req = request.Dto;

            var data = Tbl_Block.Create(req.UserId,
                req.BlockName, req.BlockPramukh,req.PartyId, req.Mobile,req.Address, 
                req.CategoryId, req.CastId, req.OccupationId,imagePath);

            return await _repo.AddAsync(data, cancellationToken);

        }
    }
}
