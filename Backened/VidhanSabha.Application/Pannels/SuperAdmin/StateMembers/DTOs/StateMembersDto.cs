using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Domain.Entities.SuperAdmin;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.DTOs
{
    
    public class CreateStateMembersReqDto
    {

        public int DesignationId { get;   set; }
        public int DesignationTypeId { get;   set; }
        public string Name { get;   set; }
        public string Email { get;   set; }
        public string Mobile { get;   set; }
        public int CategoryId { get;   set; }
        public int CastId { get;   set; }
        public IFormFile? Profile { get;   set; }
        public string Education { get;   set; }
        public DateOnly DOB { get;   set; }
        public string Address { get;   set; }
        public string Proffesion { get;   set; }

        
    }

    public class UpdateStateMembersReqDto:CreateStateMembersReqDto
    {
        public int Id { get; set; }
    }

    public class StateMembersResponseDto()
    {
        public int Id { get; set; }
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public int DesignationTypeId { get; set; }
        public string DesignationypeName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int CastId { get; set; }
        public string CastName { get; set; }
        public string Profile { get; set; }
        public string Education { get; set; }
        public DateOnly DOB { get; set; }
        public string Address { get; set; }
        public string Proffesion { get; set; }
    }



}
