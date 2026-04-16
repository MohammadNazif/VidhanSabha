using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Common.District.DTOs;
using VidhanSabha.Domain.Entities.Common;

namespace VidhanSabha.Application.Common.District.Queries
{
    public class GetAllDistrictQuery:IRequest<List<DistrictResponseDto>>
    {
        public int Id { get; set; }
        public GetAllDistrictQuery(int id) 
        {
            Id = id;
        }

        
    }
}
