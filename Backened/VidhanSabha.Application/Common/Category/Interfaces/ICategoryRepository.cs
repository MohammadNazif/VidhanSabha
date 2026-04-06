using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Application.Common.Category.Interfaces
{
    public interface  ICategoryRepository
    {
        Task<List<Tbl_Category>> GetAllAsync();
    }
}
