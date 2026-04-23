using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidhanSabha.Application.Pannels.Admin.Booth.Command;

namespace VidhanSabha.Application.Pannels.Admin.BDC.Command
{
    public class UpdateBDCValidator : AbstractValidator<UpdateBDCCommand>
    {
        public UpdateBDCValidator() 
        {
            RuleFor(x => x.Dto.Id)
                .GreaterThan(0)
                .OverridePropertyName("Id");

            RuleFor(x => x.Dto.Block)
                .NotEmpty()
                .MaximumLength(200)
                .OverridePropertyName("Block");

            RuleFor(x => x.Dto.Name)
                .NotEmpty()
                .MaximumLength(200)
                .OverridePropertyName("Name");

            RuleFor(x => x.Dto.WardNumber)
                .NotEmpty()
                .MaximumLength(200)
                .OverridePropertyName("WardNumber");

            //RuleForEach(x => x.Dto.Villages).ChildRules(v =>
            //{
            //    v.RuleFor(x => x.VillageId)
            //        .GreaterThan(0)
            //        .OverridePropertyName("VillageId");
            //}).OverridePropertyName("Villages");

            RuleFor(x => x.Dto.CategoryId)
                .GreaterThan(0)
                .OverridePropertyName("CategoryId");

            RuleFor(x => x.Dto.CastId)
                .GreaterThan(0)
                .OverridePropertyName("CastId");

            RuleFor(x => x.Dto.Age)
                .InclusiveBetween(18, 100)
                .OverridePropertyName("Age");

            RuleFor(x => x.Dto.Mobile)
                   .NotEmpty()
                   .Matches(@"^[0-9]{10}$")
                   .WithMessage("10-digit phone required.")
                   .OverridePropertyName("Mobile");

            RuleFor(x => x.Dto.PartyId)
                .GreaterThan(0)
                .OverridePropertyName("PartyId");

            RuleFor(x => x.Dto.Education)
                .NotEmpty()
                .MaximumLength(200)
                .OverridePropertyName("Education");
        }
    }
}
