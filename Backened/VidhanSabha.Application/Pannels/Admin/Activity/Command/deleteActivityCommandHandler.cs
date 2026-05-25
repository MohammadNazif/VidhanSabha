using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Exceptions;
using VidhanSabha.Application.Pannels.Admin.Activity.Interface;
using VidhanSabha.Application.Pannels.Admin.NewVoter.Command;

namespace VidhanSabha.Application.Pannels.Admin.Activity.Command
{
    public class deleteActivityCommandHandler:IRequestHandler<deleteActivityCommand,int>
    {
        private readonly IActivityRepository _repo;

        public deleteActivityCommandHandler(IActivityRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(deleteActivityCommand request, CancellationToken cancellationtoken)
        {
            var activity = await _repo.GetByIdAsync(request.Id);
            if (activity == null)
            {
                throw new NotFoundException("Activity Not Found");
            }
            activity.Delete();
            await _repo.UpdateAsync(activity);
            return activity.Id;
        }
    }
}
