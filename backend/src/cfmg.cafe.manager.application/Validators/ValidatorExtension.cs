using FluentValidation;
using FluentValidation.Results;

namespace Cfmg.Cafe.Manager.Application.Validators
{
    public static class ValidatorExtension
    {
        public static void Validate<T, TValidator>(this T t)
            where TValidator : AbstractValidator<T>
        {
            var validator = Activator.CreateInstance<TValidator>();
            ValidationResult result = validator.Validate(t);

            if (!result.IsValid)
            {
                throw new ArgumentException(string.Join(Environment.NewLine, result.Errors.Select(e => e.ErrorMessage)));
            }

        }
    }
}
