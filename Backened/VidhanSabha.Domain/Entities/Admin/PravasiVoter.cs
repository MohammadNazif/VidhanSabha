using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Domain.Entities.Admin
{
    public class Tbl_PravasiVoter
    {
        public int Id { get; private set; }
        public int BoothId { get; private set; }
        public string Name { get; private set; }
        public string Mobile { get; private set; }
        public int CategoryId { get; private set; }
        public int CastId { get; private set; }
        public int OccupationId { get; private set; }
        public string VoterId { get; private set; }
        public string CurrentAddress { get; private set; }
        public bool Status { get; private set; } = true;
        public string? UserId { get; private set; }
        public string? CreatedToUserId { get; private set; }
        public string? CreatedsectorUserId { get; private set; }
        public string? Role { get; private set; }

        private readonly List<Tbl_PravasiVillage> _villages = new();
        public IReadOnlyCollection<Tbl_PravasiVillage> Villages => _villages.AsReadOnly();


        //Navigations
        public Tbl_Booth Booth { get; private set; } = null!;
        public Tbl_Cast? Cast { get; private set; }
        public Tbl_Occupation? Occupation { get; private set; }
        public Tbl_Category? Category { get; private set; }
        private Tbl_PravasiVoter() { }

        public static Tbl_PravasiVoter Create(
            int BoothId, string Name,
            string Mobile, int CategoryId,
            int CastId, int OccupationId,
            string VoterId, string CurrentAddress, string UserId,string createdToUserId,string createdsectorUserId, string role,
            List<int> villageIds
            )
        {
            var pravasi = new Tbl_PravasiVoter
            {
                BoothId = BoothId,
                Name = Name,
                Mobile = Mobile,
                CategoryId = CategoryId,
                CastId = CastId,
                OccupationId = OccupationId,
                VoterId = VoterId,
                CurrentAddress = CurrentAddress,
                UserId = UserId,
                CreatedToUserId = createdToUserId,
                CreatedsectorUserId = createdsectorUserId,
                Role = role
            };
            pravasi.SetVillages(villageIds);
            return pravasi;
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
                _villages.Add(Tbl_PravasiVillage.Create(vid)); // ✅ EF sees this as INSERT
        }
        public void Update(
            int BoothId, string Name,
            string Mobile, int CategoryId,
            int CastId, int OccupationId,
            string VoterId, string CurrentAddress,
            List<int> villageIds
            )
        {
            this.BoothId = BoothId;
            this.Name = Name;
            this.Mobile = Mobile;
            this.CategoryId = CategoryId;
            this.CastId = CastId;
             this.OccupationId = OccupationId;
            this.VoterId = VoterId;
            this.CurrentAddress = CurrentAddress;
            SetVillages(villageIds);
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
    

    public class Tbl_PravasiVillage
    {
        public int Id { get; private set; }
        public int VillageId { get; private set; }
        public int PravasiId { get; private set; }
        public bool Status { get; private set; } = true;

        //Navigations
        public Tbl_Village? Village { get; private set; }
        public Tbl_PravasiVoter Pravasi { get; set; }
        private Tbl_PravasiVillage() { }

        public static Tbl_PravasiVillage Create(int villageId) => new()
        {
            VillageId = villageId
        };
        public void Delete()
        {
            Status = false;
        }
       

    }
}
