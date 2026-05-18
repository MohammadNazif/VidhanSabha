using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.BoothSamiti.Dtos
{
    // 🔹 CREATE
    public class CreateBoothSamitiRequestDto
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int CasteId { get; set; }
        public int Age { get; set; }
        public string Contact { get; set; }
        public string Occupation { get; set; }
        public int DesignationId { get; set; }
        public int BoothIdMem { get; set; }
    }

    // 🔹 RESPONSE (GET ke liye)
    public class BoothSamitiResponseDto
    {
       public List<Members> Members { get; set; }
       public SanyojakResponseDto BoothSanyojak { get; set; }
    }

    public class Members
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int CasteId { get; set; }
        public string CasteName { get; set; }
        public int Age { get; set; }
        public string Contact { get; set; }
        public string Occupation { get; set; }

        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
    }

    public class UpdateBoothSamitiRequestDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int CasteId { get; set; }
        public int Age { get; set; }
        public string Contact { get; set; }
        public string Occupation { get; set; }
        public int DesignationId { get; set; }
    }


    public class CreateBoothSamitiMemRequestDto
    {
        public int BoothId { get; set; }
    }

    public class BoothSamitiMemResponseDto
    {
        public int Id { get; set; }
        public int BoothId { get; set; }

        public string Designation { get; set; } 
        public string CastName { get; set; } 
        public string CategoryName { get; set; } 
        public int BoothNo { get; set; }
        public int Age { get; set; }
        public string Village { get; set; }
        public string PollingStation { get; set; }
        public string BoothAdhayaksh { get; set; }
        public int TotalMember { get; set; }
        public string Contact { get; set; } 
    }
}
