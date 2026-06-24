using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Auth;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Domain.Entities.Admin
{

    public class Tbl_Booth
    {
        public int Id { get; private set; }
        public string? UserId { get; private set; }
        public string? CreatedToSectorUserId { get; private set; }
        public string? Role { get; private set; }
        public int MandalId { get; private set; }
        public int SectorId { get; private set; }
        public int BoothNumber { get; private set; }
        public string PollingStationName { get; private set; }
        public string PollingStationLocation { get; private set; }
        public bool IsBoothSanyojak { get; private set; }

        public bool Status { get; private set; } = true;
        public Tbl_Sector Sector { get; private set; }
        public Tbl_Mandal Mandal { get; private set; }
        public Tbl_LoginCredential? Login { get; private set; }
      


        private readonly List<Tbl_BoothVillage> _villages = new();
        public IReadOnlyCollection<Tbl_BoothVillage> Villages => _villages.AsReadOnly();

        // null when IsBoothSanyojak = false
        public Tbl_BoothSanyojak? Sanyojak { get; private set; }

        private Tbl_Booth() { }

        public static Tbl_Booth Create(
            string UserId,string role,string sectorUserId, int mandalId, int sectorId, int boothNumber,
            string pollingStationName, string pollingStationLocation,
            bool isBoothSanyojak, List<Tbl_BoothVillage> villages,
            Tbl_BoothSanyojak? sanyojak)
        {
            var booth = new Tbl_Booth
            {
                UserId=UserId,
                Role  = role,
                CreatedToSectorUserId=sectorUserId,
                MandalId = mandalId,
                SectorId = sectorId,
                BoothNumber = boothNumber,
                PollingStationName = pollingStationName,
                PollingStationLocation = pollingStationLocation,
                IsBoothSanyojak = isBoothSanyojak,
                Sanyojak = isBoothSanyojak ? sanyojak : null   // ← key rule
            };
            booth._villages.AddRange(villages);
            return booth;
        }

        public void Update(
        int mandalId, int sectorId, int boothNumber,
        string pollingStationName, string pollingStationLocation,
        bool isBoothSanyojak, List<Tbl_BoothVillage> villages,
        Tbl_BoothSanyojak? sanyojak)
        {
            MandalId = mandalId;
            SectorId = sectorId;
            BoothNumber = boothNumber;
            PollingStationName = pollingStationName;
            PollingStationLocation = pollingStationLocation;
            IsBoothSanyojak = isBoothSanyojak;

            // 🔹 Sanyojak logic
            if (!isBoothSanyojak)
            {
                Sanyojak = null;
            }
            else
            {
                if (Sanyojak == null)
                {
                    Sanyojak = sanyojak;
                }
                else
                {
                    Sanyojak.UpdateProfile(

                        sanyojak.InchargeName,
                        sanyojak.Age,
                        sanyojak.FatherName,
                        sanyojak.CategoryId,
                        sanyojak.CastId,
                        sanyojak.EducationLevel,
                        sanyojak.PhoneNumber,
                        sanyojak.Address,
                        sanyojak.ProfileImagePath
                    );
                }
            }


            _villages.Clear();
            _villages.AddRange(villages);
        }

        public void Delete()
        {
            Status = false;

            // delete sanyojak if exists
            if (Sanyojak != null)
            {
                Sanyojak.Delete();
            }

            // delete villages
            foreach (var village in _villages)
            {
                village.Delete();
            }
        }
    }

    // ── Village join entity ──────────────────────────────────────
    public class Tbl_BoothVillage
    {
        public int Id { get; private set; }
        public int BoothId { get; private set; }
        public int VillageId { get; private set; }

        public bool HasAnshik { get; private set; }

        public bool Status { get; private set; } = true;
        public Tbl_Village? Village { get; private set; }
        public Tbl_Booth Booth { get; set; }
        private Tbl_BoothVillage() { }

        public static Tbl_BoothVillage Create(int villageId, bool hasAnshik)
            => new()
            {
                VillageId = villageId,
                HasAnshik = hasAnshik,
            };

        public void Delete()
        {
            Status = false;
        }
    }


    public class Tbl_BoothSanyojak
    {
        public int Id { get; private set; }
        public string InchargeName { get; private set; }
        public int Age { get; private set; }

        public string? UserId { get; set; }
        public int BoothId { get; private set; }
        public string FatherName { get; private set; }
        public int CategoryId { get; private set; }
        public int CastId { get; private set; }
        public string? EducationLevel { get; private set; }
        public string PhoneNumber { get; private set; }
        public string? Address { get; private set; }
        public string? ProfileImagePath { get; private set; }
        public Tbl_Cast? Cast { get; private set; }
        public Tbl_Category? Category { get; private set; }

        public Tbl_Booth? Booth { get; set; }

        public Boolean Status { get; private set; } = true;
        private Tbl_BoothSanyojak() { }

        public static Tbl_BoothSanyojak Create(string userId,
            string inchargeName, int age, string fatherName,
            int categoryId, int castId, string? educationLevel,
            string phoneNumber, string? address,string? profileimagepath)
            => new()
            {
                UserId = userId,
                InchargeName = inchargeName,
                Age = age,
                FatherName = fatherName,
                CategoryId = categoryId,
                CastId = castId,
                EducationLevel = educationLevel,
                PhoneNumber = phoneNumber,
                Address = address,
                ProfileImagePath = profileimagepath
            };

        public void SetImage(string path) => ProfileImagePath = path;

        public void UpdateProfile(
        string inchargeName,
         int age,
         string fatherName,
         int categoryId,
        int castId,
        string? educationLevel,
         string phoneNumber,
        string? address,
            string? profileimagepath)
        {
            // validation (important)
            if (string.IsNullOrWhiteSpace(inchargeName))
                throw new Exception("Name required");

            if (age <= 0)
                throw new Exception("Invalid age");

            InchargeName = inchargeName;
            Age = age;
            FatherName = fatherName;
            CategoryId = categoryId;
            CastId = castId;
            EducationLevel = educationLevel;
            PhoneNumber = phoneNumber;
            Address = address;
            ProfileImagePath = profileimagepath;
        }

        public void Delete()
        {
            Status = false;
        }
    }
}

