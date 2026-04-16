using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.District.DTOs;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Application.Common.District.Interfaces
{
    public interface IDistrictRepository
    {
        Task<List<Tbl_District>> GetDistrictsByIdAsync(int id);
    }
}
