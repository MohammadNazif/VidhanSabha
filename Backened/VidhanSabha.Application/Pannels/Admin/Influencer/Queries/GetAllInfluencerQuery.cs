using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.Influencer.DTOs;
using VidhanSabha.Application.Pannels.Admin.PannaPramukh.Dtos;

namespace VidhanSabha.Application.Pannels.Admin.Influencer.Queries
{
    public class GetAllInfluencerQuery: IRequest<List<InfluencerResponseDto>>
    {
        
    }
}
