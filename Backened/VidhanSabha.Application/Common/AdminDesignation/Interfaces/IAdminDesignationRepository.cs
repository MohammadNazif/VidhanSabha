using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Application.Common.AdminDesignation.Interfaces
{
    public interface IAdminDesignationRepository
    {
        Task<List<Tbl_AdminDesignation>> GetAllAsync();
    }
}
