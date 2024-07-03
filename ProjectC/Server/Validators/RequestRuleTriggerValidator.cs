using FluentValidation;
using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Validators
{
    public class RequestRuleTriggerValidator : AbstractValidator<RequestRuleTrigger>
    {
        public RequestRuleTriggerValidator()
        {
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
