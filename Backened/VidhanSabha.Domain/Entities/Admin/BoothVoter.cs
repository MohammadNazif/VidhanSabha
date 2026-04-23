using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Domain.Entities.Admin
{
    public class Tbl_BoothVoter
    {
        public int Id { get; private set; }
        public int BoothId { get; private set; }
        public int TotalVoter { get; private set; }
        public int Male { get; private set; }
        public int Female { get; private set; }
        public int Other { get; private set; }
        public bool Status { get; private set; } = true;


        private readonly List<Tbl_BoothVoterVillage> _villages = new();
        public IReadOnlyCollection<Tbl_BoothVoterVillage> Villages => _villages.AsReadOnly();


        //Navigations
        public Tbl_Booth Booth { get; private set; } = null!;
        private Tbl_BoothVoter() { }

        public static Tbl_BoothVoter Create(
            int BoothId, int TotalVoter, int Male,
            int Female, int Other,
            List<int> villageIds
            )
        {
            var boothvoter = new Tbl_BoothVoter
            {
                BoothId = BoothId,
                TotalVoter = TotalVoter,
                Male = Male,
                Female = Female,
                Other = Other
            };
            boothvoter.SetVillages(villageIds);
            return boothvoter;
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
                _villages.Add(Tbl_BoothVoterVillage.Create(vid)); // ✅ EF sees this as INSERT
        }

        public void Update(
            int BoothId, int TotalVoter, int Male,
            int Female, int Other,
            List<int> villageIds
            )
        {
            this.BoothId = BoothId;
            this.TotalVoter = TotalVoter;
            this.Male = Male;
            this.Female = Female;
            this.Other = Other;
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

    public class Tbl_BoothVoterVillage
    {
        public int Id { get; private set; }
        public int VillageId { get; private set; }
        public int BoothVoterId { get; private set; }
        public bool Status { get; private set; } = true;

        //Navigations
        public Tbl_Village? Village { get; private set; }

        public Tbl_BoothVoter? BoothVoter { get; set; }
        private Tbl_BoothVoterVillage() { }

        public static Tbl_BoothVoterVillage Create(int villageId) => new()
        {
            VillageId = villageId
        };
        public void Delete()
        {
            Status = false;
        }
    }
}
