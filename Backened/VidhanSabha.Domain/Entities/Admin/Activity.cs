using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Domain.Entities.Admin
{
    public class Tbl_Activity
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = default!;
        public string? UserId { get; private set; } = default!;
        public DateTime Date { get; private set; }
        public string Description { get; private set; } = default!;
        public string? YouTubeLink { get; private set; }
        public string? VideoPath { get; private set; }
        public bool Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private readonly List<Tbl_ActivityImage> _images = new();
        public IReadOnlyCollection<Tbl_ActivityImage> Images => _images.AsReadOnly();

        private Tbl_Activity() { }

        public static Tbl_Activity Create(
            string userId,
            string title,
            DateTime date,
            string description,
            string? youTubeLink,
            string? videoPath,
            List<Tbl_ActivityImage> images)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required.", nameof(title));
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description is required.", nameof(description));
            if (images.Count > 4)
                throw new ArgumentException("Cannot attach more than 4 images.");

            var entity = new Tbl_Activity
            {
                UserId = userId,
                Title = title,
                Date = date,
                Description = description,
                YouTubeLink = youTubeLink,
                VideoPath = videoPath,
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
            entity._images.AddRange(images);
            return entity;
        }

        public void Update(
            string title,
            DateTime date,
            string description,
            string? youTubeLink,
            string? videoPath,
            List<Tbl_ActivityImage> newImages)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required.", nameof(title));
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description is required.", nameof(description));
            if (newImages.Count > 4)
                throw new ArgumentException("Cannot attach more than 4 images.");

            Title = title;
            Date = date;
            Description = description;
            YouTubeLink = youTubeLink;
            VideoPath = videoPath;
            UpdatedAt = DateTime.UtcNow;

            _images.Clear();
            _images.AddRange(newImages);
        }

        public void Delete() => Status = false;
    }

    public class Tbl_ActivityImage
    {
        public int Id { get; private set; }
        public int ActivityId { get; private set; }
        public string ImagePath { get; private set; } = default!;
        public int SortOrder { get; private set; }

        private Tbl_ActivityImage() { }

        public static Tbl_ActivityImage Create(string imagePath, int sortOrder) =>
            new()
            {
                ImagePath = imagePath,
                SortOrder = sortOrder
            };
    }
}