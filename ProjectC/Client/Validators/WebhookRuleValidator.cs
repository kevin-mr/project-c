using FluentValidation;
using ProjectC.Shared.Models;

namespace ProjectC.Client.Validators
{
    public class WebhookRuleValidator : AbstractValidator<WebhookRuleDto>
    {
        public WebhookRuleValidator()
        {
            RuleFor(x => x.Path).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue =>
            async (model, propertyName) =>
            {
                var result = await ValidateAsync(
                    ValidationContext<WebhookRuleDto>.CreateWithOptions(
                        (WebhookRuleDto)model,
                        x => x.IncludeProperties(propertyName)
                    )
                );
                return result.IsValid
                    ? (IEnumerable<string>)Array.Empty<string>()
                    : result.Errors.Select(e => e.ErrorMessage);
            };
    }
}
