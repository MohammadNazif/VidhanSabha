using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Domain.Entities.Admin
{
    public class Tbl_BoothSamiti
    {
        public int Id { get; private set; }

        public string Name { get; private set; }
        public int CategoryId { get; private set; }
        public int CasteId { get; private set; }
        public int Age { get; private set; }
        public string Contact { get; private set; }
        public string Occupation { get; private set; }
        public int DesignationId { get; private set; }
        public bool Status { get; private set; } = true;
        public string? UserId { get; private set; }
        public int BoothIdMem { get; private set; }

        // 🔗 Common se aayega
        public Tbl_BoothSamitiDesignation Designation { get; private set; } = null!;
        public Tbl_BoothSamitiMem BoothSamitiMem { get; private set; }
        public Tbl_Category Category { get; private set; } = null;
        public Tbl_Cast Caste { get; private set; } = null;

        private Tbl_BoothSamiti() { }

        public static Tbl_BoothSamiti Create(
            string name, int category, int caste,
            int age, string contact, string occupation, int designationId,string UserId,int boothIdMem)
        {
            return new Tbl_BoothSamiti
            {
                Name = name,
                CategoryId = category,
                CasteId = caste,
                Age = age,
                Contact = contact,
                Occupation = occupation,
                DesignationId = designationId,
                UserId = UserId,
                BoothIdMem = boothIdMem,
            };
        }

        public void Update(
            string name, int category, int caste,
            int age, string contact, string occupation, int designationId)
        {
            Name = name;
            CategoryId = category;
            CasteId = caste;
            Age = age;
            Contact = contact;
            Occupation = occupation;
            DesignationId = designationId;
        }

        public void Delete()
        {
            Status = false;
        }
    }
    public class Tbl_BoothSamitiMem
    {
        public int Id { get; private set; }
        public int BoothId { get; private set; }
        public int TotalMembers { get; private set; } = 0;
        public bool Status { get; private set; } = true;
        public string? UserId { get; private set; }

        // 🔗 Common se aayega
        public Tbl_Booth Booth { get; private set; } = null;
        //public Tbl_BoothSanyojak BoothSanyojak { get; private set; } = null;
        //public Tbl_BoothVillage BoothVillage { get; private set; } = null;

        private Tbl_BoothSamitiMem() { }

        public static Tbl_BoothSamitiMem Create(
            int boothId, string UserId)
        {
            return new Tbl_BoothSamitiMem
            {
                BoothId = boothId,
                UserId = UserId
            };
        }

        public void Increment()
        {
            TotalMembers++;
        }

        public void Decrement()
        {
            if (TotalMembers > 0)
                TotalMembers--;
        }
    }
}
