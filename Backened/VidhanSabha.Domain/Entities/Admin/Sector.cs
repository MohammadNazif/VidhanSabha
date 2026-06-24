using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Auth;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Domain.Entities.Admin
{
    public class Tbl_Sector
    {
        public int Id { get; private set; }
        public int MandalId { get; private set; }
        //public int? VillageId { get; private set; }
        public string SectorName { get; private set; }
        public bool IsSectorSanyojak { get; private set; }

        private readonly List<Tbl_SectorVillage>? _villages = new();
        public IReadOnlyCollection<Tbl_SectorVillage>? Villages => _villages.AsReadOnly();

        public string? UserId { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedByUserId { get; set; }
        public int? CreatedById { get; private set; }

        // Only filled if IsSectorSanyojak = true
        public string? InchargeName { get; private set; }
        public int? Age { get; private set; }
        public string? FatherName { get; private set; }
        public int? CategoryId { get; private set; }
        public int CastId { get; private set; }
        public string? EducationLevel { get; private set; }
        public string? PhoneNumber { get; private set; }
        public string? Address { get; private set; }
        public string? ProfileImage { get; private set; }
        public bool Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        // Navigation
        public Tbl_Mandal? Mandal { get; private set; }
        //public Tbl_Village? Village { get; private set; }
        public Tbl_Booth? Booth { get; private set; }
        public Tbl_Category? Category { get; private set; }
        public Tbl_Cast? Cast { get; private set; }
        public Tbl_LoginCredential? Login  { get; private set; }

        private Tbl_Sector() { } // EF Core

        // Factory - No Sanyojak
        public static Tbl_Sector CreateBasic(
            int createdById,
            string? createdBy,
            string createdByUserId,
            int mandalId,
            List<int>? villageIds,
            string sectorName)
        {
            if (string.IsNullOrWhiteSpace(sectorName))
                throw new ArgumentException("Sector name is required.");

            var sector= new Tbl_Sector
            {
                CreatedById = createdById,
                CreatedBy = createdBy,
                CreatedByUserId = createdByUserId,
                MandalId = mandalId,
                SectorName = sectorName,
                IsSectorSanyojak = false,
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
            sector.SetVillages(villageIds);
            return sector;
        }

        // Factory - With Sanyojak
        public static Tbl_Sector CreateWithSanyojak(
            int createdById,
            string? createdBy,
            string createdByUserId,
            int mandalId,
            List<int>? villageIds,
            string sectorName,
            string userId,
            string inchargeName,
            int age,
            string fatherName,
            int categoryId,
            int castId,
            string educationLevel,
            string phoneNumber,
            string address,
            string? profileImage)
        {
            if (string.IsNullOrWhiteSpace(sectorName))
                throw new ArgumentException("Sector name is required.");
            if (string.IsNullOrWhiteSpace(inchargeName))
                throw new ArgumentException("Incharge name is required.");
            if (age <= 0 || age > 120)
                throw new ArgumentException("Invalid age.");
            if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length != 10)
                throw new ArgumentException("Invalid phone number.");

            var sector= new Tbl_Sector
            {
                UserId =userId,
                CreatedById = createdById,
                CreatedBy = createdBy,
                CreatedByUserId = createdByUserId,
                MandalId = mandalId,
                
                SectorName = sectorName,
                IsSectorSanyojak = true,
                InchargeName = inchargeName,
                Age = age,
                FatherName = fatherName,
                CategoryId = categoryId,
                CastId = castId,
                EducationLevel = educationLevel,
                PhoneNumber = phoneNumber,
                Address = address,
                ProfileImage = profileImage,
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
            sector.SetVillages(villageIds);
            return sector;
        }

        // Update - No Sanyojak
        public void UpdateBasic(int mandalId, List<int>? villageIds, string sectorName)
        {
            MandalId = mandalId;
            SetVillages(villageIds);
            SectorName = sectorName;
            IsSectorSanyojak = false;
            InchargeName = null;
            Age = null;
            FatherName = null;
            CategoryId = null;
            CastId = 0;
            EducationLevel = null;
            PhoneNumber = null;
            Address = null;
            ProfileImage = null;
            UpdatedAt = DateTime.UtcNow;
        }

        // Update - With Sanyojak
        public void UpdateWithSanyojak(
            int mandalId,
            List<int>? villageIds,
            string sectorName,
            string inchargeName,
            int age,
            string fatherName,
            int categoryId,
            int castId,
            string educationLevel,
            string phoneNumber,
            string address,
            string? profileImage)
        {
            MandalId = mandalId;
            SetVillages(villageIds);
            SectorName = sectorName;
            IsSectorSanyojak = true;
            InchargeName = inchargeName;
            Age = age;
            FatherName = fatherName;
            CategoryId = categoryId;
            CastId = castId;
            EducationLevel = educationLevel;
            PhoneNumber = phoneNumber;
            Address = address;
            ProfileImage = profileImage ?? ProfileImage;
            UpdatedAt = DateTime.UtcNow;
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
                _villages.Add(Tbl_SectorVillage.Create(vid)); // ✅ EF sees this as INSERT
        }


        public void Delete() => Status = false;
    }

    public class Tbl_SectorVillage
    {
        public int Id { get; private set; }
        public int VillageId { get; private set; }
        public int SectorId { get; private set; }
        public bool Status { get; private set; } = true;

        //Navigations
        public Tbl_Village? Village { get; private set; }
        public Tbl_Sector Sector { get; set; }
        private Tbl_SectorVillage() { }

        public static Tbl_SectorVillage Create(int villageId) => new()
        {
            VillageId = villageId
        };
        public void Delete()
        {
            Status = false;
        }


    }
}
