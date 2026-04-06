using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace VidhanSabha.Application.Pannels.Admin.Mandal.Commands
{
    public class CreateMandalCommandValidator
         : AbstractValidator<CreateMandalCommand>
    {
        public CreateMandalCommandValidator()
        {
            RuleFor(x => x.VidhanId)
                .GreaterThan(0).WithMessage("VidhanId must be greater than 0.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Mandal name is required.")
                .MaximumLength(255).WithMessage("Mandal name cannot exceed 255 characters.");
        }
    }
}
