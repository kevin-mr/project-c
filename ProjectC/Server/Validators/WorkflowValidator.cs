using FluentValidation;
using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Validators
{
    public class WorkflowValidator : AbstractValidator<Workflow>
    {
        public WorkflowValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
