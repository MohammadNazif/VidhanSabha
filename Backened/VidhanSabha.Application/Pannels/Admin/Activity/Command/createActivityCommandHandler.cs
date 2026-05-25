using MediatR;
using VidhanSabha.Application.Common.ImageService.Interface;
using VidhanSabha.Application.Common.UnitOfWork;
using VidhanSabha.Application.Pannels.Admin.Activity.Command;
using VidhanSabha.Application.Pannels.Admin.Activity.Interface;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.NewsEvent.Handlers
{
    public class createActivityCommandHandler : IRequestHandler<createActivityCommand, int>
    {
        private readonly IActivityRepository _repo;
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _uow;

        public createActivityCommandHandler(
            IActivityRepository repo,
            IImageService imageService,
            IUnitOfWork uow)
        {
            _repo = repo;
            _imageService = imageService;
            _uow = uow;
        }

        public async Task<int> Handle(createActivityCommand request, CancellationToken ct)
        {
            var dto = request.Dto;

            // Validate: cannot provide both YouTube link and a video file
            if (!string.IsNullOrWhiteSpace(dto.YouTubeLink) && dto.VideoFile is not null)
                throw new InvalidOperationException("Provide either a YouTube link OR a video file, not both.");

            // Validate image count
            if (dto.Images is not null && dto.Images.Count > 4)
                throw new InvalidOperationException("You can upload a maximum of 4 images.");

            await _uow.BeginTransactionAsync();
            try
            {
                // ── Step 1: Save video (if uploaded) ─────────────────────────────
                string? videoPath = null;
                if (dto.VideoFile is not null)
                    videoPath = await _imageService.SaveImageAsync(dto.VideoFile, "activity/videos");

                // ── Step 2: Save images (up to 4) ────────────────────────────────
                var imageEntities = new List<Tbl_ActivityImage>();
                if (dto.Images is { Count: > 0 })
                {
                    for (int i = 0; i < dto.Images.Count; i++)
                    {
                        var path = await _imageService.SaveImageAsync(dto.Images[i], "activity/images");
                        if (path is not null)
                            imageEntities.Add(Tbl_ActivityImage.Create(path, sortOrder: i));
                    }
                }

                // ── Step 3: Build aggregate ───────────────────────────────────────
                var newsEvent = Tbl_Activity.Create(
                    dto.UserId,
                    dto.Title,
                    dto.Date,
                    dto.Description,
                    dto.YouTubeLink,
                    videoPath,
                    imageEntities);

                var result = await _repo.AddAsync(newsEvent, ct);

                await _uow.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                throw new ApplicationException(
                    $"Activity creation failed. Rolled back. Reason: {ex.Message}", ex);
            }
        }
    }
}