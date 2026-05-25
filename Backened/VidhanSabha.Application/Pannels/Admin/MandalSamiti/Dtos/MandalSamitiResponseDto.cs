using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using VidhanSabha.Application.Common.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.MandalSamiti.Dtos
{
    public class MandalSamitiResponseDto 
    {
        public int Id { get; set; }
        public int MandalId { get; set; }
        public string MandalName { get; set; }
        public string MandalAdhayaksh { get; set; }
        public int TotalMember { get; set; }
        public string Contact { get; set; }
    }

    public class MandalSamitiQueryParams : BaseQueryParams
    {
     
    }
    public class CreateMandalSamitiMemberRequestDto
    {
        public int MemberId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int CasteId { get; set; }
        public int Age { get; set; }
        public string Contact { get; set; }
        public string Occupation { get; set; }
        public int DesignationId { get; set; }
        public int MandalId { get; set; }

    }
    public class UpdateMandalSamitiMemberRequestDto
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

    public class  MandalSamitiMemberResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Occupation { get; set; }
        public int CasteId { get; set; }
        public string CasteName { get; set; }
        public int Age { get; set; }
        public string Contact { get; set; }
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public int MandalId { get; set; }

    }

    public class MandalSamitiDesignationResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
    }





    //public class MandalSamitiReqDto
    //{
    //    public int MandalId { get; set; }
    //    public int TotalMember { get; set; }


    //}
}
