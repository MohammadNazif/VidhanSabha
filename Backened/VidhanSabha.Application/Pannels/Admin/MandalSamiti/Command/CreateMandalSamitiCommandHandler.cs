using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Charts;
using MediatR;
using VidhanSabha.Application.Pannels.Admin.Booth.Interfaces;
using VidhanSabha.Application.Pannels.Admin.MandalSamiti.Interfaces;
using VidhanSabha.Domain.Entities.Admin;
using VidhanSabha.Domain.Enums;

namespace VidhanSabha.Application.Pannels.Admin.MandalSamiti.Command
{
    internal class CreateMandalSamitiCommandHandler : IRequestHandler<CreateMandalSamitiCommand, int>
    {
        private IMandalSamiti _repo;
        private IBoothRepository _booth;

        public CreateMandalSamitiCommandHandler(IMandalSamiti repo,IBoothRepository booth)
        {
            _repo = repo;
            _booth = booth;
        }
        public async Task<int> Handle(CreateMandalSamitiCommand request, CancellationToken cancellationToken)
        {
            string createdtouserId = null;
            string createdsectorUserId = null;
            //if (IsUserRole(request.Role, PrabhariRole.VidhanSabhaPrabhari))
            //{
            //    createdsectorUserId = await _booth.GetSectorUseridbyBoothId(request.BoothId);
            //    createdtouserId = await _booth.GetUseridbyBoothId(request.BoothId);
            //}
            //else if (IsUserRole(request.Role, PrabhariRole.BoothSanyojak))
            //{
            //    createdtouserId = request.UserId;
            //    createdsectorUserId = await _booth.GetSectorUseridbyBoothId(request.BoothId);

            //    request.UserId = await _booth.GetadminUseridbyUserId(request.BoothId);

            //}
            //else if (IsUserRole(request.Role, PrabhariRole.SectorSanyojak))
            //{
            //    createdsectorUserId = request.UserId;
            //    createdtouserId = await _booth.GetUseridbyBoothId(request.BoothId);
            //    request.UserId = await _booth.GetadminUseridbyUserId(request.BoothId);

            //}

            // Get existing MandalSamiti - use MandalId to query since Id is a FK
            var existingMandalSamiti = await _repo.GetMandalSamitiByIdAsync(request.MandalId, cancellationToken);
            
            if (existingMandalSamiti != null)
            {
                throw new InvalidOperationException($"Mandal Samiti for Mandal ID {request.MandalId} already exists.");
            }

            // NOTE: To create a MandalSamiti, you must first ensure a MandalSanyojak exists with the same ID.
            // The Id property of MandalSamiti is a foreign key that references MandalSanyojak.Id.
            // This should be handled by your domain/business logic or a separate command.
            // For now, using the MandalId as a placeholder - adjust based on your actual business logic.
            
            var entity = Tbl_MandalSamiti.Create(request.MandalId, request.UserId, createdtouserId, createdsectorUserId, request.Role);

            return await _repo.InsertMandalSamiti(entity);


        }

        private bool IsUserRole(string? role, PrabhariRole targetRole)
        {
            return string.Equals(role, targetRole.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
