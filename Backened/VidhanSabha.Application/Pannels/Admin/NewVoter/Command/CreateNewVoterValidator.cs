using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhanSabha.Application.Pannels.Admin.NewVoter.Command
{
    public class CreateNewVoterValidator: AbstractValidator<CreateNewVoterCommand>
    {
        public CreateNewVoterValidator()
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

            RuleFor(x => x.Dto.Name)
                .NotEmpty()
                .MaximumLength(200)
                .OverridePropertyName("Name");

            RuleFor(x => x.Dto.FatherName)
                .NotEmpty()
                .MaximumLength(200)
                .OverridePropertyName("FatherName");

            RuleFor(x => x.Dto.Mobile)
                   .NotEmpty()
                   .Matches(@"^[0-9]{10}$")
                   .WithMessage("10-digit phone required.")
                   .OverridePropertyName("Mobile");

            RuleFor(x => x.Dto.CategoryId)
                .GreaterThan(0)
                .OverridePropertyName("CategoryId");

            RuleFor(x => x.Dto.CastId)
                .GreaterThan(0)
                .OverridePropertyName("CastId");

            RuleFor(x => x.Dto.DOB)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("Date of Birth cannot be in the future.");

            RuleFor(x => x.Dto.DOB)
                .Must(dob =>
                    {
                        var today = DateOnly.FromDateTime(DateTime.Today);
                        var age = today.Year - dob.Year;
                        if (dob > today.AddYears(-age))
                            age--;
                            return age >= 18 && age <= 100;
                    })
                .WithMessage("Age must be between 18 and 100 years.")
                .OverridePropertyName("DOB");


            RuleFor(x => x.Dto.Age)
                .InclusiveBetween(18, 100)
                .OverridePropertyName("Age");

            RuleFor(x => x.Dto.VoterId)
               .NotEmpty()
               .MaximumLength(200)
               .OverridePropertyName("VoterId");
        }
    }
}
