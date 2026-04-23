using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.DoubleVoter.Command
{
    public class CreateDoubleVoterValidator:AbstractValidator<CreateDoubleVoterCommand>
    {
        public CreateDoubleVoterValidator() 
        {
            RuleFor(x => x.Dto.BoothId)
                .GreaterThan(0)
                .OverridePropertyName("BoothId");

            //RuleForEach(x => x.Dto.VillageId).ChildRules(v =>
            //{
            //    v.RuleFor(x => x.VillageId)
            //        .GreaterThan(0)
            //        .OverridePropertyName("VillageId");
            //}).OverridePropertyName("Villages");

            RuleFor(x => x.Dto.Name)
                .NotEmpty()
                .MaximumLength(200)
                .OverridePropertyName("Name");

            RuleFor(x => x.Dto.FatherName)
                .NotEmpty()
                .MaximumLength(200)
                .OverridePropertyName("FatherName");

            RuleFor(x => x.Dto.VoterId)
                .NotEmpty()
                .MaximumLength(200)
                .OverridePropertyName("VoterId");

            RuleFor(x => x.Dto.PreviousAddress)
                .NotEmpty()
                .MaximumLength(200)
                .OverridePropertyName("PreviousAddress");

            RuleFor(x => x.Dto.CurrentAddress)
                .NotEmpty()
                .MaximumLength(200)
                .OverridePropertyName("CurrentAddress");

            RuleFor(x => x.Dto.Description)
                .NotEmpty()
                .MaximumLength(200)
                .OverridePropertyName("Description");

        }
    }
}
