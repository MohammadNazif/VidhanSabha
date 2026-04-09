using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Booth.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Queries
{
    public record GetAllBoothsQuery(int? MandalId, int? SectorId) : IRequest<List<BoothResponseDto>>;



}
