using FluentValidation;
using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Validators
{
    public class WorkflowActionValidator : AbstractValidator<WorkflowAction>
    {
        public WorkflowActionValidator()
        {
            RuleFor(x => x.Path)
                .NotEmpty()
                .When(x => x.RequestRuleId is null || x.RequestRuleId == 0);
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.ResponseStatus).InclusiveBetween(200, 600);
            RuleFor(x => x.ResponseDelay).InclusiveBetween(0, 5000);
        }
    }
}
