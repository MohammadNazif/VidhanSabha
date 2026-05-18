using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Domain.Entities.Admin
{
    public class Tbl_SocialMediaPlatform
    {
        public int Id { get; private set; }
        public string Platform { get; private set; }
        public bool Status { get; private set; } = true;
    }

    public class Tbl_SocialMediaPost
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string? PostImagePath { get; private set; }
        public string Description { get; private set; }
        public string UserId { get; private set; }
        public bool Status { get; private set; } = true;

        private readonly List<Tbl_SocialPostPlatform> _platforms = new();
        public IReadOnlyCollection<Tbl_SocialPostPlatform> Platforms => _platforms.AsReadOnly();

        private readonly List<Tbl_SocialMediaBooth> _booths = new();
        public IReadOnlyCollection<Tbl_SocialMediaBooth> Booths => _booths.AsReadOnly();

        private readonly List<Tbl_SocialMediaSector> _sectors = new();
        public IReadOnlyCollection<Tbl_SocialMediaSector> Sectors => _sectors.AsReadOnly();

        private Tbl_SocialMediaPost() { }

        public static Tbl_SocialMediaPost Create(string userId,
            string Title,string? PostImagePath,string Description,
            List<int> platformIds,List<int> boothIds,List<int> sectorIds
            )
        {
            var social = new Tbl_SocialMediaPost
            {
                UserId = userId,
                Title = Title,
                PostImagePath = PostImagePath,
                Description = Description,
            };
            social.SetPlatforms(platformIds);
            social.SetBooths(boothIds);
            social.SetSectors(sectorIds);
            return social;
        }

        private void SetPlatforms(List<int> platformIds)
        {
            // Step 1: Remove villages that are no longer in the new list
            var toRemove = _platforms
                .Where(v => !platformIds.Contains(v.PlatformId))
                .ToList();

            foreach (var v in toRemove)
                _platforms.Remove(v); // ✅ EF sees this as DELETE

            // Step 2: Add only new villages not already present
            var existingIds = _platforms.Select(v => v.PlatformId).ToHashSet();

            foreach (var vid in platformIds.Where(id => !existingIds.Contains(id)))
                _platforms.Add(Tbl_SocialPostPlatform.Create(vid)); // ✅ EF sees this as INSERT
        }

        private void SetBooths(List<int> boothIds)
        {
            // Step 1: Remove villages that are no longer in the new list
            var toRemove = _booths
                .Where(v => !boothIds.Contains(v.BoothId))
                .ToList();

            foreach (var v in toRemove)
                _booths.Remove(v); // ✅ EF sees this as DELETE

            // Step 2: Add only new villages not already present
            var existingIds = _booths.Select(v => v.BoothId).ToHashSet();

            foreach (var vid in boothIds.Where(id => !existingIds.Contains(id)))
                _booths.Add(Tbl_SocialMediaBooth.Create(vid)); // ✅ EF sees this as INSERT
        }
        private void SetSectors(List<int> sectorIds)
        {
            // Step 1: Remove villages that are no longer in the new list
            var toRemove = _sectors
                .Where(v => !sectorIds.Contains(v.SectorId))
                .ToList();

            foreach (var v in toRemove)
                _sectors.Remove(v); // ✅ EF sees this as DELETE

            // Step 2: Add only new villages not already present
            var existingIds = _sectors.Select(v => v.SectorId).ToHashSet();

            foreach (var vid in sectorIds.Where(id => !existingIds.Contains(id)))
                _sectors.Add(Tbl_SocialMediaSector.Create(vid)); // ✅ EF sees this as INSERT
        }

        public void Update(
            string Title, string? PostImagePath, string Description,
            List<int> platformIds, List<int> boothIds, List<int> sectorIds
            )
        {
            this.Title = Title;
            this.PostImagePath = PostImagePath;
            this.Description = Description;
            SetPlatforms(platformIds);
            SetBooths(boothIds);
            SetSectors(sectorIds);
        }

        public void Delete()
        {
            Status = false;
            foreach (var booth in _booths)
            {
                booth.Delete();
            }
            foreach (var sector in _sectors)
            {
                sector.Delete();
            }
        }
    }

    public class Tbl_SocialPostPlatform
    {
        public int Id { get; private set; }
        public int SocialMediaPostId { get; private set; }
        public int PlatformId { get; private set; }
        public bool Status { get; private set; } = true;

        // Navigations

        public Tbl_SocialMediaPlatform? Platform { get; private set; }
        public Tbl_SocialMediaPost? SocialMediaPost { get; private set; }

        private Tbl_SocialPostPlatform() { }
        public static Tbl_SocialPostPlatform Create(int platformId) => new()
        {
            PlatformId = platformId
        };
        public void Delete()
        {
            Status = false;
        }
    }

    public class Tbl_SocialMediaBooth
    {
        public int Id { get; private set; }
        public int SocialMediaPostId { get; private set; }
        public int BoothId { get; private set; }
        public bool Status { get; private set; } = true;

        // Navigations

        public Tbl_Booth? Booth { get; private set; }
        public Tbl_SocialMediaPost? SocialMediaPost { get; private set; }

        private Tbl_SocialMediaBooth() { }
        public static Tbl_SocialMediaBooth Create(int boothId) => new()
        {
            BoothId = boothId
        };
        public void Delete()
        {
            Status = false;
        }
    }

    public class Tbl_SocialMediaSector
    {
        public int Id { get; private set; }
        public int SocialMediaPostId { get; private set; }
        public int SectorId { get; private set; }
        public bool Status { get; private set; } = true;

        // Navigations

        public Tbl_Sector? Sector { get; private set; }
        public Tbl_SocialMediaPost? SocialMediaPost { get; private set; }

        private Tbl_SocialMediaSector() { }

        public static Tbl_SocialMediaSector Create(int sectorId) => new()
        {
            SectorId = sectorId
        };
        public void Delete()
        {
            Status = false;
        }

    }

}
