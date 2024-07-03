using FluentValidation;
using ProjectC.Shared.Models;

namespace ProjectC.Client.Validators
{
    public class RequestRuleVariantDtoValidator : AbstractValidator<RequestRuleVariantDto>
    {
        public RequestRuleVariantDtoValidator()
        {
            RuleFor(x => x.Path)
                .NotEmpty()
                .When(x => x.RequestRuleId is null || x.RequestRuleId == 0);
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.ResponseStatus).InclusiveBetween(200, 600);
            RuleFor(x => x.ResponseDelay).InclusiveBetween(0, 5000);
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue =>
            async (model, propertyName) =>
            {
                var result = await ValidateAsync(
                    ValidationContext<RequestRuleVariantDto>.CreateWithOptions(
                        (RequestRuleVariantDto)model,
                        x => x.IncludeProperties(propertyName)
                    )
                );
                return result.IsValid
                    ? (IEnumerable<string>)Array.Empty<string>()
                    : result.Errors.Select(e => e.ErrorMessage);
            };
    }
}
