using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.ImageService;
using VidhanSabha.Application.Common.ImageService.Interface;
using VidhanSabha.Application.Pannels.Admin.BDC.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Command;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.BDC.Command
{
    public class CreateBDCCommandHandler:IRequestHandler<CreateBDCCommand,int>
    {
        private readonly IImageService _imageService;
        private IBDCRepository _repo;

        public CreateBDCCommandHandler(IBDCRepository repo,IImageService imageService)
        {
            _imageService = imageService;
            _repo = repo;
        }
        public async Task<int> Handle(CreateBDCCommand request, CancellationToken cancellationToken)
        {
            var imagePath = await request.Dto.ResolveImageAsync(
            _imageService,
            subFolder: "profiles/BDC",
            imageSelector: dto => dto.Profile
        );

            var req = request.Dto;

            var data = Tbl_BDC.Create(req.UserId,
                req.BlockId, req.Name,req.WardNumber, req.CategoryId, req.CastId,req.Age, 
                req.Mobile,req.PartyId,req.Education,imagePath,
                req.VillageId);

            return await _repo.AddAsync(data, cancellationToken);

        }
    }
}
