using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Domain.Enums
{
    public enum ModulePermission
    {
       PannaPramukh = 1,
       NewVoter = 2,
       BoothVoterDescrition = 3,
       DoubleVoter = 4,
       PravashiVoter = 5,
       BoothSamiti = 6,
       EffectivePersion = 7,
       Activity = 8,
       SeniororDisabled = 9,
       Booth=10,
       SocialMediaPost = 11
    }

    public enum UserRole
    {
        Admin = 1,
        BoothSanjoyak = 2,
        PannaPramukh = 3,
    }
}
