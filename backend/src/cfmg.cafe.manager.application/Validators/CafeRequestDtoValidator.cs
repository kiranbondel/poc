using FluentValidation;
using Cfmg.Cafe.Manager.Application.Models.Dto;

namespace Cfmg.Cafe.Manager.Application.Validators
{
    public class CafeRequestDtoValidator : AbstractValidator<CafeRequestDto>
    {
        public CafeRequestDtoValidator()
        {
            RuleFor(x => x.Id)
           .Must(iSValidGuid)
           .When(x => !string.IsNullOrEmpty(x.Id))
           .WithMessage("Id must be a valid GUID.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(6, 10).WithMessage("Name must be between 6 and 10 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .Length(25, 256).WithMessage("Description must be between 25 and 256 characters.");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required.")
                .Length(5, 100).WithMessage("Location must be between 5 and 100 characters.");
        }

        private bool iSValidGuid(string id)
        {
            return Guid.TryParse(id, out _);
        }
    }
}
