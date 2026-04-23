using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace VidhanSabha.Application.Pannels.Admin.Influencer.Command
{
    public class DeleteInfluencerCommand : IRequest<int>
    {
        public int Id { get; set; }

        public DeleteInfluencerCommand(int id)
        {
            Id = id;
        }
    }
}