using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Domain.Entities.Common
{
    public class Tbl_MemberModulePermissions
    {

        public int Id { get; private set; }

        public string MemberId { get; private set; }
        public ModulePermission Module { get; private set; }
        public bool hasPermission { get; private set; }

        private Tbl_MemberModulePermissions() { } 


        public static Tbl_MemberModulePermissions Create(string memberId, ModulePermission module, bool hasPermission)
        {
        
            return new Tbl_MemberModulePermissions
            {
                MemberId = memberId,
                Module = module,
                hasPermission = hasPermission
            };
        }

    }
}
