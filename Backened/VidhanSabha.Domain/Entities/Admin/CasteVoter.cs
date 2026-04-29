using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Domain.ValueObjects;

namespace VidhanSabha.Domain.Entities.Admin
{
    public class Tbl_CasteVoter
    {
        public int Id { get; private set; }
        public int CasteVoterId  { get; private set; }
        public int SubCasteId { get; private set; }
        public int Number { get; private set; }
        public bool Status { get; private set; } = true;


        //Navigations
        public Tbl_BoothVoter BoothVoter { get; private set; } = null!;
        public Tbl_Cast? Cast { get; private set; }
        private Tbl_CasteVoter() { }

        public static Tbl_CasteVoter Create(
            int CasteVoterId , int SubCasteId, int Number)
        {
            var castevoter = new Tbl_CasteVoter
            {
                CasteVoterId  = CasteVoterId ,
                SubCasteId = SubCasteId,
                Number = Number,

            };
            return castevoter;
        }

        public void Update(
           int CasteVoterId , int SubCasteId, int Number)
        {
            this.CasteVoterId  = CasteVoterId ;
            this.SubCasteId = SubCasteId;
            this.Number = Number;

        }
        public void Delete()
        {
            Status = false;
        }     
    }
}
