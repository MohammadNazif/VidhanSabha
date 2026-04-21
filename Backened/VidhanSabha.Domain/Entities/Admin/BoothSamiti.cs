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
        public string Category { get; private set; }
        public string Caste { get; private set; }
        public int Age { get; private set; }
        public string Contact { get; private set; }
        public string Occupation { get; private set; }
        public int DesignationId { get; private set; }
        public bool Status { get; private set; } = true;

        // Navigation
        public Tbl_DesignationType Designation { get; private set; } = null!;

        // Constructor
        private Tbl_BoothSamiti() { }

        public static Tbl_BoothSamiti Create(
            string name,
            string category,
            string caste,
            int age,
            string contact,
            string occupation,
            int designationId)
        {
            return new Tbl_BoothSamiti
            {
                Name = name,
                Category = category,
                Caste = caste,
                Age = age,
                Contact = contact,
                Occupation = occupation,
                DesignationId = designationId
            };
        }

        public void Update(
            string name,
            string category,
            string caste,
            int age,
            string contact,
            string occupation,
            int designationId)
        {
            Name = name;
            Category = category;
            Caste = caste;
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
}
