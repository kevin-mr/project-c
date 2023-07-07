using FluentValidation;
using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Validators
{
    public class RequestRuleValidator : AbstractValidator<RequestRule>
    {
        public RequestRuleValidator()
        {
            RuleFor(x => x.Path).NotEmpty();
            RuleFor(x => x.ResponseStatus).InclusiveBetween(200, 600);
            RuleFor(x => x.ResponseDelay).InclusiveBetween(0, 5000);
        }
    }
}
