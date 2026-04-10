using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace VidhanSabha.Application.Pannels.Admin.Booth.Command
{
    public class updateBoothValidator : AbstractValidator<updateBoothCommand>
    {
        public updateBoothValidator()
        {
            RuleFor(x => x.Dto.MandalId)
                .GreaterThan(0)
                .OverridePropertyName("MandalId");

            RuleFor(x => x.Dto.SectorId)
                .GreaterThan(0)
                .OverridePropertyName("SectorId");

            RuleFor(x => x.Dto.BoothNumber)
                .GreaterThan(0)
                .OverridePropertyName("BoothNumber");

            RuleFor(x => x.Dto.PollingStationName)
                .NotEmpty()
                .MaximumLength(200)
                .OverridePropertyName("PollingStationName");

            RuleFor(x => x.Dto.PollingStationLocation)
                .NotEmpty()
                .MaximumLength(300)
                .OverridePropertyName("PollingStationLocation");

            RuleFor(x => x.Dto.Villages)
                .NotEmpty()
                .OverridePropertyName("Villages");

            // Village
            RuleForEach(x => x.Dto.Villages).ChildRules(v =>
            {
                v.RuleFor(x => x.VillageId)
                    .GreaterThan(0)
                    .OverridePropertyName("VillageId");
            }).OverridePropertyName("Villages");

            // Sanyojak
            When(x => x.Dto.IsBoothSanyojak, () =>
            {
                RuleFor(x => x.Dto.Sanyojak)
                    .NotNull()
                    .OverridePropertyName("Sanyojak");

                RuleFor(x => x.Dto.Sanyojak!.InchargeName)
                    .NotEmpty()
                    .MaximumLength(100)
                    .OverridePropertyName("InchargeName");

                RuleFor(x => x.Dto.Sanyojak!.Age)
                    .InclusiveBetween(18, 100)
                    .OverridePropertyName("Age");

                RuleFor(x => x.Dto.Sanyojak!.FatherName)
                    .NotEmpty()
                    .MaximumLength(100)
                    .OverridePropertyName("FatherName");

                RuleFor(x => x.Dto.Sanyojak!.CategoryId)
                    .GreaterThan(0)
                    .OverridePropertyName("CategoryId");

                RuleFor(x => x.Dto.Sanyojak!.CastId)
                    .GreaterThan(0)
                    .OverridePropertyName("CastId");

                RuleFor(x => x.Dto.Sanyojak!.PhoneNumber)
                    .NotEmpty()
                    .Matches(@"^[0-9]{10}$")
                    .WithMessage("10-digit phone required.")
                    .OverridePropertyName("PhoneNumber");
            });
        }
    }
}
