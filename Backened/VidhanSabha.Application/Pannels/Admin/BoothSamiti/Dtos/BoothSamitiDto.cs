using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

    // 🔹 RESPONSE (GET ke liye)
    public class BoothSamitiResponseDto
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

    // 🔹 UPDATE (Pradhan jaisa inherit karega)
    public class UpdateBoothSamitiRequestDto : CreateBoothSamitiRequestDto
    {
        public int Id { get; set; }
    }
}
