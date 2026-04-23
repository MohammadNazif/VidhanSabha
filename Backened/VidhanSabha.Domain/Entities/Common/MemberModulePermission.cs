using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Domain.Entities.Common
{
    public class Tbl_MemberModulePermission
    {

        public int Id { get; private set; }

        public int MemberId { get; private set; }
        public string Module { get; private set; } = string.Empty;
        public bool hasPermission { get; private set; }

        private Tbl_MemberModulePermission() { } // EF Core


        public static Tbl_MemberModulePermission Create(int memberId, string module, bool hasPermission)
        {
            if (string.IsNullOrWhiteSpace(module))
                throw new ArgumentException("Module name is required.");
            return new Tbl_MemberModulePermission
            {
                MemberId = memberId,
                Module = module,
                hasPermission = hasPermission
            };
        }

    }
}
