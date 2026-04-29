using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Domain.Entities.SuperAdmin
{
    public class Tbl_StateMembers
    {
        public int Id { get; private set; }
        public int DesignationId { get; private set; }
        public int DesignationTypeId { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Mobile { get; private set; }
        public int CategoryId { get; private set; }
        public int CastId { get; private set; }
        public string? Profile { get; private set; }
        public string Education { get; private set; }
        public DateOnly DOB { get; private set; }
        public string Address { get; private set; }
        public string Proffesion { get; private set; }
        public bool Status { get; private set; } = true;

        public Tbl_Designation Designation { get; private set; }
        public Tbl_DesignationType DesignationType { get; private set; }
        public Tbl_Category Category { get; private set; }
        public Tbl_Cast Cast { get; private set; }


        private Tbl_StateMembers() { }

        public static Tbl_StateMembers Create(
            int designationId,int desingationTypeId,string name,string email,string mobile,
            int categoryId,int castId,string profile,string education,DateOnly dob,string address,string proffesion
            )
        {
            return new Tbl_StateMembers
            {
                DesignationId = designationId,
                DesignationTypeId = desingationTypeId,
                Name = name,
                Email = email,
                Mobile = mobile,
                CategoryId = categoryId,
                CastId = castId,
                Profile = profile,
                Education = education,
                DOB = dob,
                Address = address,
                Proffesion = proffesion
            };
        }

        public void Update(
            int designationId, int desingationTypeId, string name, string email, string mobile,
            int categoryId, int castId, string profile, string education, DateOnly dob, string address, string proffesion
            )
        {
            DesignationId = designationId;
            DesignationTypeId = desingationTypeId;
            Name = name;
            Email = email;
            Mobile = mobile;
            CategoryId = categoryId;
            CastId = castId;
            Profile = profile;
            Education = education;
            DOB = dob;
            Address = address;
            Proffesion = proffesion;
        }
        public void Delete()
        {
            Status = false;
        }
    }
}
