using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Domain.Entities.SuperAdmin;

namespace VidhanSabha.Domain.Entities.StatePrabhari
{
    public class Tbl_VidhanSabha
    {
        public int Id { get;private set; }

        public int StateId { get; set; }
        public string UserId { get; private set; }
        public string VidhanSabhaName { get; private set; }
        public int VidhanSabhaNumber { get; private set; }

     
        public int DistrictId { get; private set; }
        //public Tbl_StatePrabhari? Prabhari { get; private set; }

        public Tbl_District district { get; set; }

        public bool Status { get; private set; } = true;

        private Tbl_VidhanSabha()
        {

        }

        public static Tbl_VidhanSabha Create(string vidhanSabhaName, int VidhanSabhaNumber, int districtId,string userId,int stateId)
        {
            if (string.IsNullOrWhiteSpace(vidhanSabhaName))
                throw new ArgumentException("VidhanSabhaName cannot be empty.");
            if (VidhanSabhaNumber <= 0)
                throw new ArgumentException("VidhanSabhaCount must be greater than zero.");
            if (districtId <= 0)
                throw new ArgumentException("Select Valid District.");
            return new Tbl_VidhanSabha
            {
                VidhanSabhaName = vidhanSabhaName,
                VidhanSabhaNumber = VidhanSabhaNumber,
                DistrictId = districtId,
                UserId = userId,
                StateId = stateId
            };


        }
    }
}
