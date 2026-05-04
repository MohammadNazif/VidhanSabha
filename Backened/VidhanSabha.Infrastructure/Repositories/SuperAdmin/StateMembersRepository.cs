using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.Dtos;
using VidhanSabha.Application.Pannels.Admin.PravasiVoters.DTOs;
using VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.DTOs;
using VidhanSabha.Application.Pannels.SuperAdmin.StateMembers.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Entities.SuperAdmin;
using VidhanSabha.Infrastructure.Extensions;
using VidhanSabha.Infrastructure.Persistence;
using VidhanSabha.Infrastructure.Repositories.Common;

namespace VidhanSabha.Infrastructure.Repositories.SuperAdmin
{
    public class StateMembersRepository:BaseRepository<Tbl_StateMembers>,IStateMembersRepository
    {
        public StateMembersRepository(DatabaseContext context) : base(context)
        {

        }

        public async Task<int> AddAsync(Tbl_StateMembers members, CancellationToken ct = default)
        {
            try
            {
                _context.Tbl_StateMembers.Add(members);
                return await _context.SaveChangesAsync(ct);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int Update(Tbl_StateMembers members)
        {
            try
            {
                _context.Tbl_StateMembers.Update(members);
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void Delete(Tbl_StateMembers members)
        {
            throw new NotImplementedException();
        }

        public async Task<Tbl_StateMembers?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Tbl_StateMembers.Where(m => m.Id == id ).FirstOrDefaultAsync();
        }


        public async Task<PagedResult<StateMembersResponseDto>> GetAllAsync(StateMembersQueryParams qp,int? samitiId, CancellationToken ct = default)
        {
            var query = _context.Tbl_StateMembers
               .AsNoTracking()
               .Where(b =>
                   (!qp.Id.HasValue || b.Id == qp.Id) && (samitiId == null || b.DesignationTypeId == samitiId)
                   );

            Expression<Func<Tbl_StateMembers, bool>>? search = null;

            if (!string.IsNullOrWhiteSpace(qp.SearchTerm))
            {
                var term = qp.SearchTerm.Trim().ToLower();
                search = b =>
                    b.Name.ToLower().Contains(term) ||
                    b.Email.ToLower().Contains(term) ||
                    b.Mobile.ToLower().Contains(term) ||
                    b.Cast.CastName.ToLower().Contains(term);
                    
            }

            return await query.ToPagedResultAsync(
               queryParams: qp,
               searchPredicate: search,
               defaultSort: b => b.Name,
               projection: m => new StateMembersResponseDto
               {
                   Id = m.Id,
                   DesignationId=m.DesignationId,
                   DesignationName=m.Designation.DesignationName,
                   DesignationTypeId=m.DesignationTypeId,
                   DesignationypeName=m.DesignationType.DesignationName,
                   Name = m.Name,
                   Email=m.Email,
                   Mobile = m.Mobile,
                   CategoryId = m.CategoryId,
                   CategoryName = m.Category.Name,
                   CastId = m.CastId,
                   CastName = m.Cast.CastName,
                   Profile=m.Profile,
                   Education=m.Education,
                   DOB=m.DOB,
                   Address=m.Address,
                   Proffesion=m.Proffesion
               },
               ct: ct
               );
        }

       
    }
}
