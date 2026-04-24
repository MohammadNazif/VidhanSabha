using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Domain.Entities.Admin
{
    public class Tbl_DoubleVoter
    {
        public int Id { get; private set; }
        public int BoothId { get; private set; }
        public string Name { get; private set; }
        public string FatherName { get; private set; }
        public string VoterId { get; private set; }
        public string PreviousAddress { get; private set; }
        public string CurrentAddress { get; private set; }
        public string Description { get; private set; }
        public bool Status { get; private set; } = true;
        public string? UserId { get; private set; }

        private readonly List<Tbl_DoubleVoterVillage> _villages = new();
        public IReadOnlyCollection<Tbl_DoubleVoterVillage> Villages => _villages.AsReadOnly();

        //Navigations
        public Tbl_Booth Booth { get; private set; } = null!;

        private Tbl_DoubleVoter() { }
        public static Tbl_DoubleVoter Create(
            int BoothId, string Name, string FatherName,
            string VoterId, string PreviousAddress,
            string CurrentAddress, string Description,string UserId, List<int> villageIds
            )
        {
            var Double = new Tbl_DoubleVoter
            {
                BoothId = BoothId,
                Name = Name,
                FatherName = FatherName,
                VoterId = VoterId,
                PreviousAddress = PreviousAddress,
                CurrentAddress = CurrentAddress,
                Description = Description,
                UserId=UserId,
                
            };
            Double.SetVillages(villageIds);
            return Double;
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
                _villages.Add(Tbl_DoubleVoterVillage.Create(vid)); // ✅ EF sees this as INSERT
        }

        public void Update(
            int BoothId, string Name, string FatherName,
            string VoterId, string PreviousAddress,
            string CurrentAddress, string Description, List<int> villageIds
            )
        {
            this.BoothId = BoothId;
            this.Name = Name;
            this.FatherName = FatherName;
            this.VoterId = VoterId;
            this.PreviousAddress = PreviousAddress;
            this.CurrentAddress = CurrentAddress;
            this.Description = Description;
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

    public class Tbl_DoubleVoterVillage
    {
        public int Id { get; private set; }
        public int VillageId { get; private set; }
        public int DoubleVoterId { get; private set; }
        public bool Status { get; private set; } = true;

        //Navigations
        public Tbl_Village? Village { get; private set; }
        public Tbl_DoubleVoter Double { get; set; }
        private Tbl_DoubleVoterVillage() { }

        public static Tbl_DoubleVoterVillage Create(int villageId) => new()
        {
            VillageId = villageId
        };
        public void Delete()
        {
            Status = false;
        }


    }

}
