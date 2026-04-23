using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Domain.Entities.Admin
{
    public class Tbl_Sector
    {
        public int Id { get; private set; }
        public int MandalId { get; private set; }
        public int VillageId { get; private set; }
        public string SectorName { get; private set; }
        public bool IsSectorSanyojak { get; private set; }

        public string? UserId { get; set; }
        public string CreatedBy { get; set; }
        public int CreatedById { get; private set; }

        // Only filled if IsSectorSanyojak = true
        public string? InchargeName { get; private set; }
        public int? Age { get; private set; }
        public string? FatherName { get; private set; }
        public int? CategoryId { get; private set; }
        public int? CastId { get; private set; }
        public string? EducationLevel { get; private set; }
        public string? PhoneNumber { get; private set; }
        public string? Address { get; private set; }
        public string? ProfileImage { get; private set; }
        public bool Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        // Navigation
        public Tbl_Mandal Mandal { get; private set; }
        public Tbl_Village Village { get; private set; }
        public Tbl_Booth Booth { get; private set; }
        public Tbl_Category Category { get; private set; }
        public Tbl_Cast Cast { get; private set; }

        private Tbl_Sector() { } // EF Core

        // Factory - No Sanyojak
        public static Tbl_Sector CreateBasic(
            int createdById,
            string? createdBy,
            int mandalId,
            int villageId,
            string sectorName)
        {
            if (string.IsNullOrWhiteSpace(sectorName))
                throw new ArgumentException("Sector name is required.");

            return new Tbl_Sector
            {
                CreatedById = createdById,
                CreatedBy = createdBy,
                MandalId = mandalId,
                VillageId = villageId,
                SectorName = sectorName,
                IsSectorSanyojak = false,
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
        }

        // Factory - With Sanyojak
        public static Tbl_Sector CreateWithSanyojak(
            int createdById,
            string? createdBy,
            int mandalId,
            int villageId,
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

            return new Tbl_Sector
            {
                UserId =userId,
                CreatedById = createdById,
                CreatedBy = createdBy,
                MandalId = mandalId,
                VillageId = villageId,
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
        }

        // Update - No Sanyojak
        public void UpdateBasic(int mandalId, int villageId, string sectorName)
        {
            MandalId = mandalId;
            VillageId = villageId;
            SectorName = sectorName;
            IsSectorSanyojak = false;
            InchargeName = null;
            Age = null;
            FatherName = null;
            CategoryId = null;
            CastId = null;
            EducationLevel = null;
            PhoneNumber = null;
            Address = null;
            ProfileImage = null;
            UpdatedAt = DateTime.UtcNow;
        }

        // Update - With Sanyojak
        public void UpdateWithSanyojak(
            int mandalId,
            int villageId,
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
            VillageId = villageId;
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

        public void Delete() => Status = false;
    }
}
