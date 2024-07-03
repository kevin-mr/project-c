using FluentValidation;
using ProjectC.Shared.Models;

namespace ProjectC.Client.Validators
{
    public class RequestRuleDtoValidator : AbstractValidator<RequestRuleDto>
    {
        public RequestRuleDtoValidator()
        {
            RuleFor(x => x.Path).NotEmpty();
            RuleFor(x => x.ResponseStatus).InclusiveBetween(200, 600);
            RuleFor(x => x.ResponseDelay).InclusiveBetween(0, 5000);
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue =>
            async (model, propertyName) =>
            {
                var result = await ValidateAsync(
                    ValidationContext<RequestRuleDto>.CreateWithOptions(
                        (RequestRuleDto)model,
                        x => x.IncludeProperties(propertyName)
                    )
                );
                return result.IsValid
                    ? (IEnumerable<string>)Array.Empty<string>()
                    : result.Errors.Select(e => e.ErrorMessage);
            };
    }
}
