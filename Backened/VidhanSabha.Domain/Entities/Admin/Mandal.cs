using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Domain.Entities.Admin
{
    // Domain/Entities/Mandal/Tbl_Mandal.cs
    public class Tbl_Mandal
    {
        private string _name;

        private Tbl_Mandal() { }

        public int Id { get; private set; }
        public int VidhanId { get; private set; }   // ← add
        public string Name { get => _name; private set => _name = value; }
        public bool Status { get; private set; }

        // Navigations

        public ICollection<Tbl_Sector>? Sectors { get; private set; }
        // ── Factory ──────────────────────────────────────────────
        public static Tbl_Mandal Create(int vidhanId, string name)
        {
            ValidateVidhanId(vidhanId);                // ← add
            ValidateName(name);

            return new Tbl_Mandal
            {
                VidhanId = 1,
                _name = name.Trim(),
                Status = true
            };
        }

        public void Update(string name)
        {
            ValidateName(name);

            _name = name;
            Status = true;
            
        }

        // ── Behaviors ────────────────────────────────────────────
        public bool IsActive() => Status;

        public void Activate()
        {
            if (Status)
                throw new InvalidOperationException("Mandal is already active.");
            Status = true;
        }

        public void Deactivate()
        {
            if (!Status)
                throw new InvalidOperationException("Mandal is already inactive.");
            Status = false;
        }

        // ── Validations ──────────────────────────────────────────
        private static void ValidateVidhanId(int vidhanId)
        {
            if (vidhanId <= 0)
                throw new ArgumentException("VidhanId must be greater than 0.");
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Mandal name is required.");
            if (name.Trim().Length > 255)
                throw new ArgumentException("Mandal name cannot exceed 255 characters.");
        }
    }
}
