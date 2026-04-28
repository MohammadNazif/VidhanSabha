using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.SocialMediaPost.Command
{
    public class DeleteSocialMediaCommand : IRequest<int>
    {
        public int Id { get; set; }

        public DeleteSocialMediaCommand(int id)
        {
            Id = id;
        }
    }
}
