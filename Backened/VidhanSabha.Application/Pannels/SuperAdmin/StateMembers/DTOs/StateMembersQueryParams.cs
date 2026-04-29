using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.DTOs
{
    public class StateMembersQueryParams:BaseQueryParams
    {
        public int? Id { get; set; }
    }
}
