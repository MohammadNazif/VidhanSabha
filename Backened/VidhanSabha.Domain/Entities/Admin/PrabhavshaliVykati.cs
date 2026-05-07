using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Domain.Entities.SuperAdmin;

namespace VidhanSabha.Domain.Entities.Admin
{
    public class Tbl_PrabhavshaliVyakti
    {
        public int Id { get; private set; }
        public int BoothId { get; private set; }
        public int DesignationId { get; private set; }
        public string Name { get; private set; }
        public int CategoryId { get; private set; }
        public int CastId { get; private set; }
        public string Mobile { get; private set; }
        public string Description { get; private set; }
        public bool Status { get; private set; } = true;
        public string? UserId { get; private set; }
        public string? CreatedToUserId { get; private set; }
        public string? CreatedsectorUserId { get; private set; }
        public string? Role { get; private set; }


        private readonly List<Tbl_PrabhavshaliVillage> _villages = new();
        public IReadOnlyCollection<Tbl_PrabhavshaliVillage> Villages => _villages.AsReadOnly();

        //Navigations
        public Tbl_Booth Booth { get; private set; } = null!;
        public Tbl_AdminDesignation Designation { get; private set; } = null!;
        public Tbl_Cast? Cast { get; private set; }
        public Tbl_Category? Category { get; private set; }

        private Tbl_PrabhavshaliVyakti() { }

        public static Tbl_PrabhavshaliVyakti Create(
            int BoothId,int DesignationId, string Name,
            int CategoryId, int CastId,
            string Mobile,string Description,string userId,string createdToUserId,string createdsectorUserId,string role,
            List<int> villageIds
            )
        {
            var prabhav = new Tbl_PrabhavshaliVyakti
            {
                BoothId = BoothId,
                DesignationId=DesignationId,
                Name = Name,
                CategoryId = CategoryId,
                CastId = CastId,
                Mobile = Mobile,
                Description=Description,
                UserId = userId,
                CreatedToUserId = createdToUserId,
                CreatedsectorUserId = createdsectorUserId,
                Role = role
            };
            prabhav.SetVillages(villageIds);
            return prabhav;
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
                _villages.Add(Tbl_PrabhavshaliVillage.Create(vid)); // ✅ EF sees this as INSERT
        }

        public void Update(
          int BoothId, int DesignationId, string Name,
            int CategoryId, int CastId,
            string Mobile, string Description,
            List<int> villageIds
           )
        {
            this.BoothId = BoothId;
            this.DesignationId = DesignationId;
            this.Name = Name;
            this.CategoryId = CategoryId;
            this.CastId = CastId;
            this.Mobile = Mobile;
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

    public class Tbl_PrabhavshaliVillage
    {
        public int Id { get; private set; }
        public int VillageId { get; private set; }
        public int PrabhavshaliId { get; private set; }
        public bool Status { get; private set; } = true;

        //Navigations
        public Tbl_Village? Village { get; private set; }
        public Tbl_PrabhavshaliVyakti Prabhavshali { get; set; }
        private Tbl_PrabhavshaliVillage() { }

        public static Tbl_PrabhavshaliVillage Create(int villageId) => new()
        {
            VillageId = villageId
        };
        public void Delete()
        {
            Status = false;
        }


    }

}
