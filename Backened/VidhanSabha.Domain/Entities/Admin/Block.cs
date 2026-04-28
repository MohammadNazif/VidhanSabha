using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Domain.Entities.Admin
{
    public class Tbl_Block
    {
        public int Id { get; private set; }
        public string BlockName { get; private set; }
        public string BlockPramukh { get; private set; }
        public int PartyId { get; private set; }
        public string Mobile { get; private set; }
        public string Address { get; private set; }
        public int CategoryId { get; private set; }
        public int CastId { get; private set; }
        public int OccupationId { get; private set; }
        public string Profile { get; private set; }
        public bool Status { get; private set; } = true;

        //Navigations
        public Tbl_Party Party { get; private set; } = null!;
        public Tbl_Cast? Cast { get; private set; }
        public Tbl_Occupation? Occupation { get; private set; }
        public Tbl_Category? Category { get; private set; }

        private Tbl_Block() { }

        public static Tbl_Block Create(
            string BlockName,string BlockPramukh,int PartyId,
            string Mobile,string Address, int CategoryId,
            int CastId,int OccupationId,string Profile
            )
        {
            var block = new Tbl_Block
            {
                BlockName = BlockName,
                BlockPramukh = BlockPramukh,
                PartyId = PartyId,
                Mobile = Mobile,
                Address = Address,
                CategoryId = CategoryId,
                CastId = CastId,
                OccupationId = OccupationId,
                Profile=Profile
            };
            return block;
        }
        public void Update(
            string BlockName, string BlockPramukh, int PartyId,
            string Mobile, string Address, int CategoryId,
            int CastId, int OccupationId,string Profile
            )
        {
            this.BlockName = BlockName;
            this.BlockPramukh = BlockPramukh;
            this.PartyId = PartyId;
            this.Mobile = Mobile;
            this.Address = Address;
            this.CategoryId = CategoryId;
            this.CastId = CastId;
            this.OccupationId = OccupationId;
            this.Profile = Profile;
        }
        public void Delete()
        {
            Status = false;
        }
    }
}
