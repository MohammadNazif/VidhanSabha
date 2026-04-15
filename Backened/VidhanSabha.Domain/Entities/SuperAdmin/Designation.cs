using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Domain.Entities.SuperAdmin
{
   
        public class Tbl_Designation
        {
            public int Id { get; private set; }
            public string DesignationName { get; private set; } = string.Empty;
            public int DesignationTypeId { get; private set; }
            public bool Status { get; private set; } = true;

            // Navigation
            public Tbl_DesignationType DesignationType { get; set; } = null!;

            private Tbl_Designation() { }

            // ── Create ───────────────────────────────────────────────
            public static Tbl_Designation Create(
                string designationName,
                int designationTypeId)
            {
                if (string.IsNullOrWhiteSpace(designationName))
                    throw new ArgumentException("Designation name required.");
                if (designationTypeId <= 0)
                    throw new ArgumentException("DesignationType invalid.");

                return new Tbl_Designation
                {
                    DesignationName = designationName.Trim(),
                    DesignationTypeId = designationTypeId,
                    Status = true
                };
            }

            // ── Update ───────────────────────────────────────────────
            public void Update(string designationName, int designationTypeId)
            {
                if (string.IsNullOrWhiteSpace(designationName))
                    throw new ArgumentException("Designation name required.");
                if (designationTypeId <= 0)
                    throw new ArgumentException("DesignationType invalid.");

                DesignationName = designationName.Trim();
                DesignationTypeId = designationTypeId;
            }

            // ── Soft Delete ──────────────────────────────────────────
            public void Delete() => Status = false;

            // ── Restore (agar kabhi chahiye) ─────────────────────────
            public void Restore() => Status = true;
        }
    }
