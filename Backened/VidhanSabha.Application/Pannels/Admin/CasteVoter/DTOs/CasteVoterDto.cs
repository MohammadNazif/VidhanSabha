using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.CasteVoter.DTOs
{
    public class CasteVoterItemDto
    {
        public int SubCasteId { get; set; }
        public int Number { get; set; }
    }
    public class CreateCasteVoterReqDto
    {
        public int CasteVoterId  { get; set; }
        public List<CasteVoterItemDto> CasteVoters { get; set; } = new();
    }
    public class CasteVoterResponseDto
    {
        public int Id { get; set; }
        public int CasteVoterId  { get; set; }
        public int BoothNumber { get; set; }
        public string PollingStationName { get; set; } = string.Empty;
        public int SubCasteId { get; set; }
        public string SubCasteName { get; set; } = string.Empty;
        public int Number { get; set; }
    }

    public class UpdateCasteVoterReqDto
    {
        public int CasteVoterId  { get; set; }
        public List<CasteVoterItemDto> CasteVoters { get; set; } = new();
    }

}
