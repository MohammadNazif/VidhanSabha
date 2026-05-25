using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Domain.Entities.Admin
{
    public class Tbl_NewVoter
    {
        public int Id { get; private set; }
        public int BoothId { get; private set; }
        public string Name { get; private set; }
        public string FatherName { get; private set; }
        public string Mobile { get; private set; }
        public int CategoryId { get; private set; }
        public int CastId { get; private set; }
        public DateOnly DOB { get; private set; }
        public int Age { get; private set; }
        public string VoterId { get; private set; }
        public bool Status { get; private set; } = true;
        public string? UserId { get; private set; }
        public string? CreatedToUserId { get; private set; }
        public string? CreatedsectorUserId { get; private set; }
        public string? Role { get; private set; }
        

        private readonly List<Tbl_NewVoterVillage> _villages = new();
        public IReadOnlyCollection<Tbl_NewVoterVillage> Villages => _villages.AsReadOnly();


        //Navigations
        public Tbl_Booth Booth { get; private set; } = null!;
        public Tbl_Cast? Cast { get; private set; }
        public Tbl_Category? Category { get; private set; }
        private Tbl_NewVoter() { }

        public static Tbl_NewVoter Create(
            int BoothId, string Name, string FatherName,
            string Mobile, int CategoryId,
            int CastId, DateOnly DOB,int Age,
            string VoterId, string UserId,string createdToUserId,string createdsectorUserId,string role,
            List<int> villageIds
            )
        {
            var newvoter = new Tbl_NewVoter
            {
                BoothId = BoothId,
                Name = Name,
                FatherName=FatherName,
                Mobile = Mobile,
                CategoryId = CategoryId,
                CastId = CastId,
                DOB=DOB,
                Age=Age,
                VoterId = VoterId,
                UserId = UserId,
                CreatedToUserId = createdToUserId,
                CreatedsectorUserId = createdsectorUserId,
                Role = role
            };
            newvoter.SetVillages(villageIds);
            return newvoter;
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
                _villages.Add(Tbl_NewVoterVillage.Create(vid)); // ✅ EF sees this as INSERT
        }

        public void Update(
            int BoothId, string Name, string FatherName,
            string Mobile, int CategoryId,
            int CastId, DateOnly DOB, int Age,
            string VoterId,
            List<int> villageIds
            )
        {
            this.BoothId = BoothId;
            this.Name = Name;
            this.FatherName = FatherName;
            this.Mobile = Mobile;
            this.CategoryId = CategoryId;
            this.DOB = DOB;
            this.Age = Age;
            this.CastId = CastId;
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

    public class Tbl_NewVoterVillage
    {
        public int Id { get; private set; }
        public int VillageId { get; private set; }
        public int NewVoterId { get; private set; }
        public bool Status { get; private set; } = true;

        //Navigations
        public Tbl_Village? Village { get; private set; }
        public Tbl_NewVoter NewVoter { get; set; }
        private Tbl_NewVoterVillage() { }

        public static Tbl_NewVoterVillage Create(int villageId) => new()
        {
            VillageId = villageId
        };
        public void Delete()
        {
            Status = false;
        }


    }

}
