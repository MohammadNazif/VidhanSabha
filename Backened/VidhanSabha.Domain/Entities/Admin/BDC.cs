using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Domain.Entities.Admin
{
    public class Tbl_BDC
    {
        public int Id { get; private set; }
        public string Block { get; private set; }
        public string Name { get; private set; }
        public string WardNumber { get; private set; }
        public int CategoryId { get; private set; }
        public int CastId { get; private set; }
        public int Age { get; private set; }
        public string Mobile { get; private set; }
        public int PartyId { get; private set; }
        public string Education { get; private set; }
        public bool Status { get; private set; } = true;

        private readonly List<Tbl_BDCVillage> _villages = new();
        public IReadOnlyCollection<Tbl_BDCVillage> Villages => _villages.AsReadOnly();

        //Navigations
        public Tbl_Party Party { get; private set; } = null!;
        public Tbl_Cast? Cast { get; private set; }
        public Tbl_Category? Category { get; private set; }

        public static Tbl_BDC Create(
            string Block, string Name,string WardNumber,
             int CategoryId,int CastId,int Age, string Mobile,
            int PartyId, string Education,
            List<int> villageIds
            )
        {
            var bdc = new Tbl_BDC
            {
                Block=Block,
                Name = Name,
                WardNumber=WardNumber,
                CategoryId = CategoryId,
                CastId = CastId,
                Age = Age,
                Mobile = Mobile,
                PartyId=PartyId,
                Education = Education,
            };
            bdc.SetVillages(villageIds);
            return bdc;
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
                _villages.Add(Tbl_BDCVillage.Create(vid)); // ✅ EF sees this as INSERT
        }

        public void Update(
            string Block, string Name, string WardNumber,
             int CategoryId, int CastId, int Age, string Mobile,
            int PartyId, string Education,
            List<int> villageIds
           )
        {
            this.Block = Block;
            this.Name = Name;
            this.WardNumber = WardNumber;
            this.CategoryId = CategoryId;
            this.CastId = CastId;
            this.Age = Age;
            this.Mobile = Mobile;
            this.PartyId = PartyId;
            this.Education = Education;
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

    public class Tbl_BDCVillage
    {
        public int Id { get; private set; }
        public int VillageId { get; private set; }
        public int BDCId { get; private set; }
        public bool Status { get; private set; } = true;

        //Navigations
        public Tbl_Village? Village { get; private set; }
        public Tbl_BDC BDC { get; set; }
        private Tbl_BDCVillage() { }

        public static Tbl_BDCVillage Create(int villageId) => new()
        {
            VillageId = villageId
        };
        public void Delete()
        {
            Status = false;
        }
    }

}
