using FluentValidation;
using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Validators
{
    public class WebhookRuleValidator : AbstractValidator<WebhookRule>
    {
        public WebhookRuleValidator()
        {
            RuleFor(x => x.Path).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
