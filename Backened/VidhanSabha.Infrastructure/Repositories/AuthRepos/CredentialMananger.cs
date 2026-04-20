using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.IdentityModel.Tokens.Experimental;
using VidhanSabha.Application.Common.CredentialMananger.Interface;
using VidhanSabha.Domain.Entities.Auth;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.AuthRepos
{
    public class CredentialMananger :  BaseRepository<Tbl_LoginCredential>, ICredentialRepository
    {
        public CredentialMananger(DatabaseContext context) : base(context)
        {
            
        }

        public async Task<Tbl_LoginCredential?> GetByUserIdAsync(string userId)
        {

            try
            {
                var entity = await _context.Tbl_LoginCredential.FirstOrDefaultAsync
                   (x => x.UserId == userId);
                return entity;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<Tbl_LoginCredential> InsertAsync(Tbl_LoginCredential login)
        {
            var entry = await _context.Tbl_LoginCredential.AddAsync(login);
            await _context.SaveChangesAsync();
            return entry.Entity;

        }

        public Task<int> SoftDeleteAsync(Tbl_LoginCredential data)
        {
            _context.Tbl_LoginCredential.Update(data);
            return  _context.SaveChangesAsync();
        }
    }
}
