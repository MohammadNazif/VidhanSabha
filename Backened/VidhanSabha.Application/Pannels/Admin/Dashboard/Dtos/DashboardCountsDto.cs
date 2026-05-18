using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.Dashboard.Dtos
{
    public class DashboardCountsDto
    {
        public int Mandal { get; set; }
        public int Sector { get; set; }
        public int Booth { get; set; }
        public int PannaPramukh { get; set; }
        public int Sahmat { get; set; }
        public int Asahmat { get; set; }
        public int Activities { get; set; }
        public int Pravasi { get; set; }
        public int NewVoters { get; set; }
        public int DoubleVoter { get; set; }
        public int PrabhavshaliVyakti { get; set; }
        public int Block { get; set; }
        public int BDC { get; set; }
        public int InfluencerPerson { get; set; }
        public int Pradhan { get; set; }
        public string vidhanSabhaName { get; set; }
        public int vidhanSabhaNumber { get; set; }
    }
    public class BoothDashboardCountsDto
    {
 
        public int BoothId { get; set; }
        public int PannaPramukh { get; set; }
        public int Sahmat { get; set; }
        public int Asahmat { get; set; }
        public int Activities { get; set; }
        public int Pravasi { get; set; }
        public int NewVoters { get; set; }
        public int DoubleVoter { get; set; }
        public int PrabhavshaliVyakti { get; set; }
        public int BoothVoter { get; set; }
        public int BoothSamiti { get; set; }
        public int VaristhNagrik { get; set; }
        public int Viklaang { get; set; }
        public int Post { get; set; }
    }
    public class SectorDashboardCountsDto
    {

        public int Booth { get; set; }
        public int SectorId { get; set; }
        public int PannaPramukh { get; set; }
        public int Sahmat { get; set; }
        public int Asahmat { get; set; }
        public int Activities { get; set; }
        public int Pravasi { get; set; }
        public int NewVoters { get; set; }
        public int DoubleVoter { get; set; }
        public int PrabhavshaliVyakti { get; set; }
        public int BoothVoter { get; set; }
        public int BoothSamiti { get; set; }
        public int VaristhNagrik { get; set; }
        public int Viklaang { get; set; }
        public int Post { get; set; }
    }
    public class StateDashboardCountsDto
    {
        public int VidhanSabha { get; set; }
        public int District { get; set; }
        public int Designation { get; set; }
        public int PradeshSamiti { get; set; }
        public int PradeshKaryarkarniSamiti { get; set; }
        public int? StateId { get; set; }
      
    }
}
