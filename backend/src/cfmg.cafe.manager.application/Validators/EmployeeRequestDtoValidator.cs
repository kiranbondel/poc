using FluentValidation;
using Cfmg.Cafe.Manager.Application.Models.Dto;

namespace Cfmg.Cafe.Manager.Application.Validators
{
    public class EmployeeRequestDtoValidator : AbstractValidator<EmployeeRequestDto>
    {
        public EmployeeRequestDtoValidator()
        {
            RuleFor(x => x.Id)
            .Matches(@"^UI[0-9A-Z]{7}$")
            .When(x => !string.IsNullOrEmpty(x.Id))
            .WithMessage("Id must be in the format 'UIXXXXXXX' where X is alphanumeric.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(6, 10).WithMessage("Name must be between 6 and 10 characters.");

            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage("EmailAddress address is required.")
                .EmailAddress().WithMessage("EmailAddress Invalid Format.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required.")
                .Must(g => g.ToUpper() == "MALE" || g.ToUpper() == "FEMALE")
                .WithMessage("Gender must be either 'Male' or 'Female'.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("PhoneNumber is required.")
                .Matches(@"^[89]\d{7}$").WithMessage("PhoneNumber must start with 8 or 9 and have 8 digits.");
        }
    }

}
