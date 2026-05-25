using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.ImageService.Interface;
using VidhanSabha.Application.Common.UnitOfWork;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Activity.Interface;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Command;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Interfaces;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.Activity.Command
{
    public class updateActivityCommandHandler:IRequestHandler<updateActivityCommand,int>
    {
        private IActivityRepository _repo;
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _uow;

        public updateActivityCommandHandler(IActivityRepository repo,
            IImageService imageService,
            IUnitOfWork uow)
        {
            _repo = repo;
            _imageService = imageService;
            _uow = uow;
        }
        public async Task<int> Handle(updateActivityCommand request, CancellationToken cancellationtoken)
        {
            var dto = request.Dto;

            // Validate: cannot provide both YouTube link and a video file
            if (!string.IsNullOrWhiteSpace(dto.YouTubeLink) && dto.VideoFile is not null)
                throw new InvalidOperationException("Provide either a YouTube link OR a video file, not both.");

            // Validate image count
            if (dto.Images is not null && dto.Images.Count > 4)
                throw new InvalidOperationException("You can upload a maximum of 4 images.");

            var activity = await _repo.GetByIdAsync(dto.Id);

            if (activity == null)
                throw new NotFoundException("Activity Not Found");

            await _uow.BeginTransactionAsync();
            try
            {
                // ── Step 1: Video handle karo ─────────────────────────────────────
                string? videoPath = activity.VideoPath; // default: purana path rakho

                if (dto.VideoFile is not null)
                    videoPath = await _imageService.UpdateImageAsync(dto.VideoFile, activity.VideoPath, "activity/videos");

                // ── Step 2: Images handle karo ────────────────────────────────────
                var imageEntities = new List<Tbl_ActivityImage>();

                if (dto.Images is { Count: > 0 })
                {
                    // Pehle saari purani images disk se delete karo
                    foreach (var oldImage in activity.Images)
                        await _imageService.DeleteImageAsync(oldImage.ImagePath);

                    // Ab nayi images save karo
                    for (int i = 0; i < dto.Images.Count; i++)
                    {
                        var path = await _imageService.SaveImageAsync(dto.Images[i], "activity/images");
                        if (path is not null)
                            imageEntities.Add(Tbl_ActivityImage.Create(path, sortOrder: i));
                    }
                }
                else
                {
                    // Koi nayi image nahi aayi, purani hi rakho
                    imageEntities = activity.Images.ToList();
                }

                // ── Step 3: Aggregate update karo ────────────────────────────────
                activity.Update(
                    dto.Title,
                    dto.Date,
                    dto.Description,
                    dto.YouTubeLink,
                    videoPath,
                    imageEntities);

                await _repo.UpdateAsync(activity);

                await _uow.CommitAsync();

                // Sirf Id return karo
                return activity.Id;
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                throw new ApplicationException(
                    $"Activity Updation failed. Rolled back. Reason: {ex.Message}", ex);
            }
        }
    }
}
