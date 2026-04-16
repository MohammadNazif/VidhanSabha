using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Interfaces;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.Admin
{
    internal class NewVoterRepository: BaseRepository<Tbl_NewVoter>, INewVoterRepository
    {
        public NewVoterRepository(DatabaseContext context) : base(context)
        {

        }
        public async Task<int> AddAsync(Tbl_NewVoter newvoter, CancellationToken ct = default)
        {
            try
            {
                await _context.Tbl_NewVoter.AddAsync(newvoter);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }


        }
    }
}
