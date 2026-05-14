using System;
using System.Collections.Generic;
using VidhanSabha.Domain.Entities.Common;
using static VidhanSabha.Domain.Entities.Admin.Tbl_Mandal;
using static VidhanSabha.Domain.Entities.Admin.Tbl_MandalSamiti;

namespace VidhanSabha.Domain.Entities.Admin
{
    public class Tbl_MandalSamitiMem
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Age { get; private set; }
        public string Contact { get; private set; }
        public string Occupation { get; private set; }
        public int DesignationId { get; private set; }
        public bool Status { get; private set; } = true;
        public int CategoryId { get; private set; }
        public int CasteId { get; private set; }
        public string? UserId { get; private set; }
        public int MandalId { get; private set; }
        public string? CreatedToUserId { get; private set; }

        // 🔗 Navigation Properties
        //public Tbl_MandalSamitiDesignation Designation { get; private set; } = null!;
        public Tbl_MandalSamiti MandalSamiti { get; private set; } = null!;
        public Tbl_MandalSamitiDesignation? designation { get; private set; } = null!;
        public Tbl_Category Category { get; private set; } = null;
        public Tbl_Cast Caste { get; private set; } = null;

        private Tbl_MandalSamitiMem() { }

        public static Tbl_MandalSamitiMem Create(
            string name, int age, string contact, string occupation,
            int designationId, int categoryId, int casteId,
            string userId, int mandalId, string createdToUserId)
        {
            return new Tbl_MandalSamitiMem
            {
                Name = name,
                Age = age,
                Contact = contact,
                Occupation = occupation,
                DesignationId = designationId,
                CategoryId = categoryId,
                CasteId = casteId,
                UserId = userId,
                MandalId = mandalId,
                CreatedToUserId = createdToUserId
            };
        }

        public void Update(
            string name, int age, string contact, string occupation,
            int designationId, int categoryId, int casteId)
        {
            Name = name;
            Age = age;
            Contact = contact;
            Occupation = occupation;
            DesignationId = designationId;
            CategoryId = categoryId;
            CasteId = casteId;
        }

        public void Delete()
        {
            Status = false;
        }
    }

    public class Tbl_MandalSamiti
    {
        public int Id { get; private set; }
        public int MandalId { get; private set; }
        public int TotalMembers { get; private set; } = 0;
        public bool Status { get; private set; } = true;
        public string? UserId { get; private set; }
        public string? CreatedToUserId { get; private set; }
        public string? CreatedsectorUserId { get; private set; }
        public string? Role { get; private set; }

        // 🔗 Navigation Properties
        public ICollection<Tbl_MandalSamitiMem> Members { get; set; }
        public Tbl_Mandal? Mandal { get; private set; } = null;
        public Tbl_MandalSanyojak? MandalSanyojak { get; private set; } = null;

        private Tbl_MandalSamiti() { }

        public static Tbl_MandalSamiti Create(
            int id, int mandalId, string userId,
            string createdToUserId, string createdsectorUserId, string role)
        {
            return new Tbl_MandalSamiti
            {
                Id = id,
                MandalId = mandalId,
                UserId = userId,
                CreatedToUserId = createdToUserId,
                CreatedsectorUserId = createdsectorUserId,
                Role = role
            };
        }

        public static Tbl_MandalSamiti Create(
            int mandalId, string userId,
            string createdToUserId, string createdsectorUserId, string role)
        {
            return new Tbl_MandalSamiti
            {
                MandalId = mandalId,
                UserId = userId,
                CreatedToUserId = createdToUserId,
                CreatedsectorUserId = createdsectorUserId,
                Role = role
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

        public void Delete()
        {
            Status = false;
        }
        public class Tbl_MandalSamitiDesignation
        {
            public int Id { get; private set; }
            public string Name { get; private set; }
            public bool Status { get; private set; } = true;

            private Tbl_MandalSamitiDesignation() { }
        }
    }
}