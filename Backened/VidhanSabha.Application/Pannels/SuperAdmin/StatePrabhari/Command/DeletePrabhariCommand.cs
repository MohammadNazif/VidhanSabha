using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace VidhanSabha.Application.Pannels.SuperAdmin.StatePrabhari.Command
{
    public class DeletePrabhariCommand : IRequest<int>
    {
        public int Id;
        public string UserId { get; set; }
        public DeletePrabhariCommand(int id,string userId)
        {
            Id = id;
            UserId = userId;
        }
    }
}
