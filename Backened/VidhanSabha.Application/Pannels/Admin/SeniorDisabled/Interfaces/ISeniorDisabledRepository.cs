using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.NewVoter.DTOs;
using VidhanSabha.Application.Pannels.Admin.SeniorDisabled.DTOs;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.SeniorDisabled.Interfaces
{
    public interface ISeniorDisabledRepository
    {
        Task<Tbl_SeniorDisabled?> GetByIdAsync(int id);
        Task<List<SeniorDisabledResponseDto>> GetAllAsync(int? boothId = null, CancellationToken ct = default);
        Task<int> AddAsync(List<Tbl_SeniorDisabled> seniordisabled, CancellationToken ct = default);
        int Update(Tbl_SeniorDisabled seniordisabled);
        void Delete(Tbl_SeniorDisabled seniordisabled);
        //Task SaveAsync(CancellationToken ct = default);
    }
}
