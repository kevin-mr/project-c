using FluentValidation;
using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Validators
{
    public class WorkflowActionValidator : AbstractValidator<WorkflowAction>
    {
        public WorkflowActionValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
