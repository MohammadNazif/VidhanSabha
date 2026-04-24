using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Domain.Entities.Admin
{
    public class Tbl_Pradhan
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int DesignationId { get; private set; }
        public string Contact { get; private set; }
        public int Gender { get; private set; }
        public bool Status { get; private set; } = true;
        private readonly List<Tbl_PradhanVillage> _villages = new();
        public IReadOnlyCollection<Tbl_PradhanVillage> Villages => _villages.AsReadOnly();

        //Navigations
        public Tbl_AdminDesignation Designation { get; private set; } = null!;
        //public Tbl_Booth Booth { get; private set; } = null!;
        public Tbl_Pradhan() { }

        public static Tbl_Pradhan Create(
             string Name, int DesignationId, string Contact, int Gender, List<int> VillageId)
        {
            var pradhan = new Tbl_Pradhan
            {
                Name = Name,
                DesignationId = DesignationId,
                Contact = Contact,
                Gender = Gender,
            };
            pradhan.SetVillages(VillageId);
            return pradhan;
        }
        private void SetVillages(List<int> VillageId)
        {
            // Step 1: Remove villages that are no longer in the new list
            var toRemove = _villages
                .Where(v => !VillageId.Contains(v.VillageId))
                .ToList();

            foreach (var v in toRemove)
                _villages.Remove(v); // ✅ EF sees this as DELETE

            // Step 2: Add only new villages not already present
            var existingIds = _villages.Select(v => v.VillageId).ToHashSet();

            foreach (var vid in VillageId.Where(id => !existingIds.Contains(id)))
                _villages.Add(Tbl_PradhanVillage.Create(vid)); // ✅ EF sees this as INSERT
        }


        public void Update(string Name, int DesignationId, string Contact, int Gender, List<int> VillageId)
        {
            this.Name = Name;
            this.DesignationId = DesignationId;
            this.Contact = Contact;
            this.Gender = Gender;
            SetVillages(VillageId);
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

        public class Tbl_PradhanVillage
    {
        public int Id { get; private set; }
        public int PradhanId { get; private set; }
        public int VillageId { get; private set; }
        public bool Status { get; private set; } = true;
        //Navigations
        public Tbl_Pradhan Pradhan { get; private set; } = null!;
        public Tbl_Village Village { get; private set; } = null!;
        private Tbl_PradhanVillage() { }

        public static Tbl_PradhanVillage Create(int VillageId) => new()
        {
            VillageId = VillageId
        };
        public void Delete()
        {
            Status = false;
        }
    }
}
