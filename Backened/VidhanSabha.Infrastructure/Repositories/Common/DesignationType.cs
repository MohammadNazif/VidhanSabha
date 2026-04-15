using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.DesignatinType.Dto;
using VidhanSabha.Application.Common.DesignatinType.Interface;
using VidhanSabha.Domain.Entities.Common;
using VidhanSabha.Infrastructure.Persistence;

namespace VidhanSabha.Infrastructure.Repositories.Common
{
    public class DesignationType : BaseRepository<Tbl_DesignationType>, IDesignationType
    {
        public DesignationType(DatabaseContext context) : base(context)
        {
            
        }
        public Task<List<DesignationTypeResponseDto>> getAllAsync(CancellationToken ct = default)
        {
            _context.Tbl_DesignationType.Select(x => new DesignationTypeResponseDto
            {
                Id = x.Id,
                DesignationName = x.DesignationName
            }).ToList();
             return Task.FromResult(_context.Tbl_DesignationType.Select(x => new DesignationTypeResponseDto
            {
                Id = x.Id,
                DesignationName = x.DesignationName
            }).ToList());
        }
    }
}
