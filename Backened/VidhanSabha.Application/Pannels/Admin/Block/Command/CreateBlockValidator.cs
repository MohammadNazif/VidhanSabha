using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.Block.Command
{
    public class CreateBlockValidator : AbstractValidator<CreateBlockCommand>
    {
        public CreateBlockValidator() 
        {
            RuleFor(x => x.Dto.BlockName)
                .NotEmpty()
                .MaximumLength(200)
                .OverridePropertyName("BlockName");

            RuleFor(x => x.Dto.BlockPramukh)
                .NotEmpty()
                .MaximumLength(200)
                .OverridePropertyName("BlockPramukh");

            RuleFor(x => x.Dto.PartyId)
                .GreaterThan(0)
                .OverridePropertyName("PartyId");

            RuleFor(x => x.Dto.Mobile)
                   .NotEmpty()
                   .Matches(@"^[0-9]{10}$")
                   .WithMessage("10-digit phone required.")
                   .OverridePropertyName("Mobile");

            RuleFor(x => x.Dto.Address)
                .NotEmpty()
                .MaximumLength(200)
                .OverridePropertyName("Address");

            RuleFor(x => x.Dto.CategoryId)
                .GreaterThan(0)
                .OverridePropertyName("CategoryId");

            RuleFor(x => x.Dto.CastId)
                .GreaterThan(0)
                .OverridePropertyName("CastId");

            RuleFor(x => x.Dto.OccupationId)
                .GreaterThan(0)
                .OverridePropertyName("OccupationId");

        }
    }
}
