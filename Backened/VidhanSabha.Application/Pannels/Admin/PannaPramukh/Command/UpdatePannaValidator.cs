using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.PannaPramukh.Command
{
    public class UpdatePannaValidator: AbstractValidator<UpdatePannaCommand>
    {
        public UpdatePannaValidator() 
        {
            RuleFor(x => x.Dto.BoothId)
                .GreaterThan(0)
                .OverridePropertyName("BoothId");

            //RuleForEach(x => x.Dto.Villages).ChildRules(v =>
            //{
            //    v.RuleFor(x => x.VillageId)
            //        .GreaterThan(0)
            //        .OverridePropertyName("VillageId");
            //}).OverridePropertyName("Villages");

            RuleFor(x => x.Dto.PannaPramukhName)
                .NotEmpty()
                .MaximumLength(200)
                .OverridePropertyName("PannaPramukhName");

            RuleFor(x => x.Dto.PannaNumber)
                .GreaterThan(0)
                .OverridePropertyName("PannaNumber");

            RuleFor(x => x.Dto.CategoryId)
                .GreaterThan(0)
                .OverridePropertyName("CategoryId");

            RuleFor(x => x.Dto.CastId)
                .GreaterThan(0)
                .OverridePropertyName("CastId");

            RuleFor(x => x.Dto.VoterId)
               .NotEmpty()
               .MaximumLength(200)
               .OverridePropertyName("VoterId");

            RuleFor(x => x.Dto.PhoneNumber)
                   .NotEmpty()
                   .Matches(@"^[0-9]{10}$")
                   .WithMessage("10-digit phone required.")
                   .OverridePropertyName("PhoneNumber");

            RuleFor(x => x.Dto.Address)
               .NotEmpty()
               .MaximumLength(300)
               .OverridePropertyName("Address");
        }
    }
}
