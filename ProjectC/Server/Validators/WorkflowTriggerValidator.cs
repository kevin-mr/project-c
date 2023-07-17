using FluentValidation;
using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Validators
{
    public class WorkflowTriggerValidator : AbstractValidator<WorkflowTrigger>
    {
        public WorkflowTriggerValidator()
        {
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
