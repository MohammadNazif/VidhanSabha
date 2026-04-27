using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.Block.DTOs
{
    public class CreateBlockReqDto
    {
        public string BlockName { get; set; }
        public string BlockPramukh { get; set; }
        public int PartyId { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public int CategoryId { get; set; }
        public int CastId { get; set; }
        public int OccupationId { get; set; }
        public IFormFile? Profile { get; init; }

    }
    public class BlockResponseDto
    {
        public int Id { get; set; }
        public string BlockName { get; set; }
        public string BlockPramukh { get; set; }
        public int PartyId { get; set; }
        public string Party { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int CastId { get; set; }
        public string CastName { get; set; }
        public int OccupationId { get; set; }
        public string Occupation { get; set; }
        public string Profile { get; set; }
    }
    public class BlockNameResponse
    {
        public int Id { get; set; }
        public string BlockName { get; set; }
    }

    public class UpdateBlockReqDto : CreateBlockReqDto
    {
        public int Id { get; set; }
    }
}
