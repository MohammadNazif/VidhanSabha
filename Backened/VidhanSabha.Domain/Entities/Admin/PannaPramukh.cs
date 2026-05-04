using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Domain.Entities.Admin
{
    public class Tbl_PannaPramukh
    {
        public int Id { get; private set; }
        public int BoothId { get; private set; }
        public int PannaNumber { get; private set; }
        public string PannaPramukhName { get; private set; }
        public int CategoryId { get; private set; }
        public int CastId { get; private set; }
        public string VoterId { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Address { get; private set; }
        public string? ProfilePicturePath { get; private set; }
        public bool Status { get; set; } = true;
        public string? UserId { get; private set; }
        public string? CreatedToUserId { get; private set; }
        public string? CreatedsectorUserId { get; private set; }
        public string? Role { get; private set; }
        

        // Multiple villages — ek Panna pramukh ke multiple villages
        private readonly List<Tbl_PannaPramukhVillage> _villages = new();
        public IReadOnlyCollection<Tbl_PannaPramukhVillage> Villages => _villages.AsReadOnly();

        // Navigation
        public Tbl_Booth Booth { get; private set; } = null!;

        public Tbl_Cast? Cast { get; private set; }
        public Tbl_Category? Category { get;private set; }
        private Tbl_PannaPramukh() { }

        public static Tbl_PannaPramukh Create(
            int boothId,
            int pannaNumber,
            string pannaPramukhName,
            int categoryId,
            int castId,
            string voterId,
            string phoneNumber,
            string address,
            string UserId,
            string createdToUserId,
            string createdsectorUserId,
            string role,
            List<int> villageIds,
            string? profilePicturePath = null)
        {
            if (boothId <= 0) throw new ArgumentException("Booth is invalid.");
            if (string.IsNullOrWhiteSpace(pannaPramukhName)) throw new ArgumentException("Name required.");
            if (!System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^[0-9]{10}$"))
                throw new ArgumentException("Phone number Should be of 10 digits.");
            if (villageIds == null || villageIds.Count == 0)
                throw new ArgumentException("Select minimum one village.");

            var panna = new Tbl_PannaPramukh
            {
                BoothId = boothId,
                PannaNumber = pannaNumber,
                PannaPramukhName = pannaPramukhName.Trim(),
                CategoryId = categoryId,
                CastId = castId,
                VoterId = voterId.ToString().Trim(),
                PhoneNumber = phoneNumber.Trim(),
                Address = address?.Trim() ?? string.Empty,
                ProfilePicturePath = profilePicturePath,
                UserId = UserId,
                CreatedToUserId = createdToUserId,
                CreatedsectorUserId = createdsectorUserId,
                Role = role
            };

            panna.SetVillages(villageIds);
            return panna;
        }

            public void Update(
                int boothId,
                int pannaNumber,
                string pannaPramukhName,
                int categoryId,
                int castId,
                string voterId,
                string phoneNumber,
                string address,
                List<int> villageIds,
                string? profilePicturePath = null)
            {
                if (string.IsNullOrWhiteSpace(pannaPramukhName)) throw new ArgumentException("Name required.");
                if (!System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^[0-9]{10}$"))
                    throw new ArgumentException("Phone number should be of 10 digits.");

                BoothId = boothId;
                PannaNumber = pannaNumber;
                PannaPramukhName = pannaPramukhName.Trim();
                CategoryId = categoryId;
                CastId = castId;
                VoterId = voterId.ToString().Trim();
                PhoneNumber = phoneNumber.Trim();
                Address = address?.Trim() ?? string.Empty;

                // Image update sirf tab jab naya aaya ho
                if (profilePicturePath is not null)
                    ProfilePicturePath = profilePicturePath;

                SetVillages(villageIds);
            }

        private void SetVillages(List<int> villageIds)
        {
            // Step 1: Remove villages that are no longer in the new list
            var toRemove = _villages
                .Where(v => !villageIds.Contains(v.VillageId))
                .ToList();

            foreach (var v in toRemove)
                _villages.Remove(v); // ✅ EF sees this as DELETE

            // Step 2: Add only new villages not already present
            var existingIds = _villages.Select(v => v.VillageId).ToHashSet();

            foreach (var vid in villageIds.Where(id => !existingIds.Contains(id)))
                _villages.Add(Tbl_PannaPramukhVillage.Create(vid)); // ✅ EF sees this as INSERT
        }

        public void Delete()
        {
            Status = false;

            foreach (var village in _villages)
            {
                village.Delete();
            }
        }
    }

    // ── Join entity — PannaPramukh ke multiple villages ──────────
    public class Tbl_PannaPramukhVillage
    {
        public int Id { get; private set; }
        public int PannaPramukhId { get; private set; }
        public int VillageId { get; private set; }

        public bool Status { get; set; } = true;
        public Tbl_PannaPramukh PannaPramukh { get; set; } = null!;
        private Tbl_PannaPramukhVillage() { }
        public Tbl_Village Village { get; set; } = null!;
        public static Tbl_PannaPramukhVillage Create(int villageId)
        {
            if (villageId <= 0) throw new ArgumentException("Village is invalid.");
            return new Tbl_PannaPramukhVillage { VillageId = villageId };
        }

        public void Delete()
        {
            Status = false;
        }
    }
}

