using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.PrabhavshaliVyakti.Command
{
    public class UpdatePrabhavValidator:AbstractValidator<UpdatePrabhavCommand>
    {
        public UpdatePrabhavValidator()
        {
            RuleFor(x => x.Dto.BoothId)
                .GreaterThan(0)
                .OverridePropertyName("BoothId");

            RuleFor(x => x.Dto.DesignationId)
                .GreaterThan(0)
                .OverridePropertyName("DesignationId");

            //RuleForEach(x => x.Dto.Villages).ChildRules(v =>
            //{
            //    v.RuleFor(x => x.VillageId)
            //        .GreaterThan(0)
            //        .OverridePropertyName("VillageId");
            //}).OverridePropertyName("Villages");

            RuleFor(x => x.Dto.Name)
                .NotEmpty()
                .MaximumLength(200)
                .OverridePropertyName("Name");

            RuleFor(x => x.Dto.CategoryId)
                .GreaterThan(0)
                .OverridePropertyName("CategoryId");

            RuleFor(x => x.Dto.CastId)
                .GreaterThan(0)
                .OverridePropertyName("CastId");

            RuleFor(x => x.Dto.Mobile)
                   .NotEmpty()
                   .Matches(@"^[0-9]{10}$")
                   .WithMessage("10-digit phone required.")
                   .OverridePropertyName("Mobile");

            RuleFor(x => x.Dto.Description)
                .NotEmpty()
                .MaximumLength(200)
                .OverridePropertyName("Description");
        }
    }
}
