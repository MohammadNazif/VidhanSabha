using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Domain.Entities.SuperAdmin
{
    public class Tbl_VidhansabhaStatewiseCount
    {
        public int Id { get; private set; }
        public int StateId { get; private set; }   // FK → common/getstates
        public int VidhansabhaCount { get; private set; }   // total seats set at creation
        public int Remainingcount { get; private set; }   // decrements on each Vidhansabha add
        public bool Status { get; private set; } = true;

        // Navigation
        public Tbl_State State { get; set; } = null!;
        //public ICollection<Tbl_Vidhansabha> Vidhansabhas { get; set; } = new List<Tbl_Vidhansabha>();

        private Tbl_VidhansabhaStatewiseCount() { }

        // ── Create ───────────────────────────────────────────────
        // Remaining = Total at creation time
        public static Tbl_VidhansabhaStatewiseCount Create(int StateId, int vidhansabhaCount)
        {
            if (StateId <= 0)
                throw new ArgumentException("Select Valid State.");
            if (vidhansabhaCount <= 0)
                throw new ArgumentException("VidhansabhaCount atleast 1.");

            return new Tbl_VidhansabhaStatewiseCount
            {
                StateId = StateId,
                VidhansabhaCount = vidhansabhaCount,
                Remainingcount = vidhansabhaCount,   // ← same as total on create
                Status = true
            };
        }

        // ── Update total count ───────────────────────────────────
        // Recalculates remaining = new total - already used seats
        public void Update(int masterStateId, int vidhansabhaCount)
        {
            if (masterStateId <= 0)
                throw new ArgumentException("Valid State Select.");
            if (vidhansabhaCount <= 0)
                throw new ArgumentException("VidhansabhaCount must be atleast 1.");

            int usedSeats = VidhansabhaCount - Remainingcount;

            if (vidhansabhaCount < usedSeats)
                throw new InvalidOperationException(
                    $"Naya count ({vidhansabhaCount}) already added Vidhansabhas ({usedSeats}) se kam nahi ho sakta.");

            StateId = masterStateId;
            VidhansabhaCount = vidhansabhaCount;
            Remainingcount = vidhansabhaCount - usedSeats;   // recalculate
        }

        // ── Called when a Vidhansabha is added ───────────────────
        public void DecrementRemaining(int remainingcount,int vidhansabhaCount)
        {
            Remainingcount = remainingcount - vidhansabhaCount;
             if (Remainingcount < 0)
                throw new InvalidOperationException(
                    "Vidhansabha Cont Could not be greater than Total Vidhansabha.");
        }

        // ── Called when a Vidhansabha is deleted/soft-deleted ────
        public void IncrementRemaining()
        {
            if (Remainingcount >= VidhansabhaCount)
                throw new InvalidOperationException(
                    "Remaining already total count ke barabar hai.");

            Remainingcount++;
        }

        // ── Soft Delete / Restore ────────────────────────────────
        public void Delete() => Status = false;
        public void Restore() => Status = true;
    }
}

