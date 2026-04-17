using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Domain.Entities.Admin
{
    public class Tbl_SahmatAsahmat
    {
        public int Id { get; private set; }
        public int BoothId { get; private set; }
        public int TypeId { get; private set; }
        public bool IsAsahmat { get; private set; }
        public string Name { get; private set; }
        public int Age { get; private set; }
        public string Mobile { get; private set; }
        public int PartyId { get; private set; }
        public int OccupationId { get; private set; }
        public string Reason { get; private set; }
        public string VoterId { get; private set; }
        public bool Status { get; private set; } = true;

        private readonly List<Tbl_SahmatAsahmatVillage> _villages = new();
        public IReadOnlyCollection<Tbl_SahmatAsahmatVillage> Villages => _villages.AsReadOnly();

        //Navigations
        public Tbl_Booth Booth { get; private set; } = null!;
        public Tbl_Party Party { get; private set; } = null!;
        public Tbl_SahmatType Type { get; private set; } = null!;
        public Tbl_Occupation? Occupation { get; private set; }

        public static Tbl_SahmatAsahmat Create(
            int BoothId,int TypeId,
            bool IsAsahmat,string Name,
            int Age,string Mobile,int PartyId,
            int OccupationId,string Reason,string VoterId, List<int> villageIds)
        {
            var data = new Tbl_SahmatAsahmat
            {
                BoothId = BoothId,
                TypeId = TypeId,
                IsAsahmat = IsAsahmat,
                Name = Name,
                Age = Age,
                Mobile = Mobile,
                PartyId = PartyId,
                OccupationId = OccupationId,
                Reason = Reason,
                VoterId = VoterId,
            };
            data.SetVillages(villageIds);
            return data;
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
                _villages.Add(Tbl_SahmatAsahmatVillage.Create(vid)); // ✅ EF sees this as INSERT
        }

        public void Update(
            int BoothId, int TypeId,
            bool IsAsahmat, string Name,
            int Age, string Mobile, int PartyId,
            int OccupationId, string Reason, string VoterId, List<int> villageIds)
        {
            this.BoothId = BoothId;
            this.TypeId = TypeId;
            this.Name = Name;
            this.Mobile = Mobile;
            this.Age = Age;
            this.PartyId = PartyId;
            this.Reason = Reason;
            this.IsAsahmat = IsAsahmat;
            this.OccupationId = OccupationId;
            this.VoterId = VoterId;
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

    public class Tbl_SahmatAsahmatVillage
    {
        public int Id { get; private set; }
        public int VillageId { get; private set; }
        public int SahmatAsahmatId { get; private set; }
        public bool Status { get; private set; } = true;

        //Navigations
        public Tbl_Village? Village { get; private set; }
        public Tbl_SahmatAsahmat SahmatAsahmat { get; set; }
        private Tbl_SahmatAsahmatVillage() { }

        public static Tbl_SahmatAsahmatVillage Create(int villageId) => new()
        {
            VillageId = villageId
        };
        public void Delete()
        {
            Status = false;
        }


    }

}
