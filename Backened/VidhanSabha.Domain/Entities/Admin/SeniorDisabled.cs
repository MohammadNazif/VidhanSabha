using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Domain.ValueObjects;

namespace VidhanSabha.Domain.Entities.Admin
{
    public class Tbl_SeniorDisabled
    {
        public int Id { get; private set; }
        public int TypeId { get; private set; }
        public int BoothId { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public int CategoryId { get; private set; }
        public int CastId { get; private set; }
        public string Mobile { get; private set; }
        public string VoterId { get; private set; }
        public bool Status { get; private set; } = true;
        public string? UserId { get; private set; }
        public string? CreatedToUserId { get; private set; }

        private readonly List<Tbl_SeniorDisabledVillage> _villages = new();
        public IReadOnlyCollection<Tbl_SeniorDisabledVillage> Villages => _villages.AsReadOnly();


        //Navigations
        public Tbl_Booth Booth { get; private set; } = null!;
        public Tbl_Cast? Cast { get; private set; }
        public Tbl_Category? Category { get; private set; }
        public Tbl_SeniorDisabledType? Type { get; private set; }

        private Tbl_SeniorDisabled() { }

        public static Tbl_SeniorDisabled Create(
     int typeId,
     int boothId,
     SeniorDisabledData data,
     string UserId,string createdToUserId,
     List<int> villageIds
 )
        {
            var senior = new Tbl_SeniorDisabled
            {
                TypeId = typeId,
                BoothId = boothId,
                Name = data.Name,
                Address = data.Address,
                CategoryId = data.CategoryId,
                CastId = data.CastId,
                Mobile = data.Mobile,
                VoterId = data.VoterId,
                UserId = UserId,
                CreatedToUserId = createdToUserId
            };

            senior.SetVillages(villageIds);
            return senior;
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
                _villages.Add(Tbl_SeniorDisabledVillage.Create(vid)); // ✅ EF sees this as INSERT
        }

        public void Update(
           int TypeId, int BoothId, string Name, string Address,
            int CategoryId, int CastId, string Mobile, string VoterId, List<int> villageIds
           )
        {
            this.TypeId = TypeId;
            this.BoothId = BoothId;
            this.Name = Name;
            this.Address = Address;
            this.CategoryId = CategoryId;
            this.CastId = CastId;
            this.Mobile = Mobile;
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

    public class Tbl_SeniorDisabledVillage
    {
        public int Id { get; private set; }
        public int VillageId { get; private set; }
        public int SeniorDisabledId { get; private set; }
        public bool Status { get; private set; } = true;

        //Navigations
        public Tbl_Village? Village { get; private set; }
        public Tbl_SeniorDisabled SeniorDisabled { get; set; }
        private Tbl_SeniorDisabledVillage() { }

        public static Tbl_SeniorDisabledVillage Create(int villageId) => new()
        {
            VillageId = villageId
        };
        public void Delete()
        {
            Status = false;
        }


    }

}
