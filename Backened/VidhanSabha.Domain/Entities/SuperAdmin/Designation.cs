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
            public string UserId { get; private set; }
            public bool Status { get; private set; } = true;

            // Navigation

            private Tbl_Designation() { }

            // ── Create ───────────────────────────────────────────────
            public static Tbl_Designation Create(
                string designationName,
                string userId
                )
            {
                if (string.IsNullOrWhiteSpace(designationName))
                    throw new ArgumentException("Designation name required.");
              

                return new Tbl_Designation
                {
                    DesignationName = designationName.Trim(),
                    UserId = userId,
                    Status = true
                };
            }

            // ── Update ───────────────────────────────────────────────
            public void Update(string designationName)
            {
                if (string.IsNullOrWhiteSpace(designationName))
                    throw new ArgumentException("Designation name required.");
                

                DesignationName = designationName.Trim();
               
            }

            // ── Soft Delete ──────────────────────────────────────────
            public void Delete() => Status = false;

            // ── Restore (agar kabhi chahiye) ─────────────────────────
            public void Restore() => Status = true;
        }


   
}
