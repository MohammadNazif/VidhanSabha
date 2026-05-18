using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Auth;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Domain.Entities.StatePrabhari;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Domain.Entities.SuperAdmin
{
    public class Tbl_StatePrabhari
    {
        public int Id { get; private set; }
        public string userId { get; set; }
        public int? StateId { get; set; }
        public int? VidhansabhaId { get; set; }
        public PrabhariRole PrabhariRole { get; private set; }
        //public int? role { get; private set; }
        public string PrabhariName { get; private set; } = string.Empty;
        public string PrabhariEmail { get; private set; } = string.Empty;
        public string Gender { get; private set; } = string.Empty;
        public string ContactNumber { get; private set; } = string.Empty;
        public int CategoryId { get; private set; }
        public int CastId { get; private set; }
        public string Education { get; private set; } = string.Empty;
        public string Profession { get; private set; } = string.Empty;
        public string? CurrentAddress { get; private set; }

        public string? CreatedByUserId { get; private set; }
        public bool Status { get; private set; } = true;
        public DateTime CreatedAt { get; private set; }

        // Navigation
        public Tbl_Category Category { get; set; } = null!;
        public Tbl_Cast Cast { get; set; } = null!;
        public Tbl_LoginCredential Login { get; set; } = null!;
        public Tbl_VidhanSabha Vidhansabha { get; set; }
        public Tbl_State State { get; set; } = null!;

        private Tbl_StatePrabhari() { }

        // ── Create ───────────────────────────────────────────────
        public static Tbl_StatePrabhari Create(
            string CreatedByUserId,
            string userId,
            int? stateId,
            int? vidhanSanhaId,
            PrabhariRole prabhariRole,
            string prabhariName,
            string prabhariEmail,
            string gender,
            string contactNumber,
            int categoryId,
            int castId,
            string education,
            string profession,
            string? currentAddress)
        {
            Validate(prabhariName, prabhariEmail, gender,
                     contactNumber, categoryId, castId, education, profession);

            return new Tbl_StatePrabhari
            {
                CreatedByUserId = CreatedByUserId,
                userId = userId,
                StateId = stateId,
                VidhansabhaId = vidhanSanhaId,
                PrabhariRole = prabhariRole,
                PrabhariName = prabhariName.Trim(),
                PrabhariEmail = prabhariEmail.Trim().ToLower(),
                Gender = gender.Trim(),
                ContactNumber = contactNumber.Trim(),
                CategoryId = categoryId,
                CastId = castId,
                Education = education.Trim(),
                Profession = profession.Trim(),
                CurrentAddress = currentAddress?.Trim(),
                Status = true,
                CreatedAt = DateTime.UtcNow
            };
        }

        // ── Update ───────────────────────────────────────────────
        public void Update(
           
            string prabhariName,
            string prabhariEmail,
            string gender,
            string contactNumber,
            int categoryId,
            int castId,
            string education,
            string profession,
            string? currentAddress)
        {
            Validate(prabhariName, prabhariEmail, gender,
                     contactNumber, categoryId, castId, education, profession);

            
            PrabhariName = prabhariName.Trim();
            PrabhariEmail = prabhariEmail.Trim().ToLower();
            Gender = gender.Trim();
            ContactNumber = contactNumber.Trim();
            CategoryId = categoryId;
            CastId = castId;
            Education = education.Trim();
            Profession = profession.Trim();
            CurrentAddress = currentAddress?.Trim();
        }

        // ── Soft Delete ──────────────────────────────────────────
        public void Delete() => Status = false;

        // ── Shared Validation ─────────────────────────────────────
          private static void Validate(
            string name, string email, string gender,
            string contact, int categoryId, int castId,
            string education, string profession)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Prabhari name required hai.");

            if (string.IsNullOrWhiteSpace(email) ||
                !System.Text.RegularExpressions.Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Valid email required hai.");

            if (gender != "Male" && gender != "Female")
                throw new ArgumentException("Gender Male ya Female hona chahiye.");

            if (!System.Text.RegularExpressions.Regex.IsMatch(contact, @"^[0-9]{10}$"))
                throw new ArgumentException("Contact number 10 digits ka hona chahiye.");

            if (categoryId <= 0)
                throw new ArgumentException("Category select karo.");

            if (castId <= 0)
                throw new ArgumentException("Caste select karo.");

            if (string.IsNullOrWhiteSpace(education))
                throw new ArgumentException("Education required hai.");

            if (string.IsNullOrWhiteSpace(profession))
                throw new ArgumentException("Profession required hai.");
        }
    }
}
