using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Domain.Entities.SuperAdmin;

namespace VidhanSabha.Domain.Entities.StatePrabhari
{
    public class Tbl_DistrictWiseCount
    {
        public int Id { get; private set; }
        public int DistrictId { get; private set; }   // FK → common/getstates
        public int VidhansabhaCount { get; private set; }

        public string UserId { get; set; }
        public int RemainingCount { get; set; }// total seats set at creation
        public bool Status { get; private set; } = true;

        // Navigation
        public Tbl_District District { get; set; } = null!;
       

        private Tbl_DistrictWiseCount() { }

        // ── Create ───────────────────────────────────────────────
        // Remaining = Total at creation time
        public static Tbl_DistrictWiseCount Create(int districtId, int vidhansabhaCount,string userId)
        {
            if (districtId <= 0)
                throw new ArgumentException("Select Valid State.");
            if (vidhansabhaCount <= 0)
                throw new ArgumentException("VidhansabhaCount atleast 1.");

            return new Tbl_DistrictWiseCount
            {
                DistrictId = districtId,
                VidhansabhaCount = vidhansabhaCount,
                RemainingCount = vidhansabhaCount,
                UserId = userId,
                Status = true
            };
        }

        // ── Update total count ───────────────────────────────────
        // Recalculates remaining = new total - already used seats
        //public void Update(int masterStateId, int vidhansabhaCount)
        //{
        //    if (masterStateId <= 0)
        //        throw new ArgumentException("Valid State Select.");
        //    if (vidhansabhaCount <= 0)
        //        throw new ArgumentException("VidhansabhaCount must be atleast 1.");

        //    int usedSeats = VidhansabhaCount - Remainingcount;

        //    if (vidhansabhaCount < usedSeats)
        //        throw new InvalidOperationException(
        //            $"Naya count ({vidhansabhaCount}) already added Vidhansabhas ({usedSeats}) se kam nahi ho sakta.");

        //    StateId = masterStateId;
        //    VidhansabhaCount = vidhansabhaCount;
        //    Remainingcount = vidhansabhaCount - usedSeats;   // recalculate
        //}

        //// ── Called when a Vidhansabha is added ───────────────────
        //public void DecrementRemaining()
        //{
        //    if (Remainingcount <= 0)
        //        throw new InvalidOperationException(
        //            "Is State mein koi seat remaining nahi hai.");

        //    Remainingcount--;
        //}

        //// ── Called when a Vidhansabha is deleted/soft-deleted ────
        //public void IncrementRemaining()
        //{
        //    if (Remainingcount >= VidhansabhaCount)
        //        throw new InvalidOperationException(
        //            "Remaining already total count ke barabar hai.");

        //    Remainingcount++;
        //}

        // ── Soft Delete / Restore ────────────────────────────────
        public void Delete() => Status = false;
        public void Restore() => Status = true;
    }
}
