using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Admin;

namespace VidhanSabha.Application.Pannels.Admin.NewVoter.Interfaces
{
    public interface INewVoterRepository
    {
        //Task<Tbl_NewVoter?> GetByIdAsync(int id);
        //Task<List<NewVoterResponseDto>> GetAllAsync(int? boothId = null, CancellationToken ct = default);
        Task<int> AddAsync(Tbl_NewVoter newvoter, CancellationToken ct = default);
        //int Update(Tbl_NewVoter pravasi);
        //void Delete(Tbl_NewVoter pravasi);
        //Task SaveAsync(CancellationToken ct = default);
    }
}
