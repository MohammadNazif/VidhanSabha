using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;
using VidhanSabha.Application.Pannels.Admin.Mandal.DTOs;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Queries
{
    public class getMandalSanyojakQuery : IRequest<MandalSanyojakDto>
    {
        public int Id { get; set; }
        public getMandalSanyojakQuery(int id)
        {
            Id = id;    
        }
    }
}
