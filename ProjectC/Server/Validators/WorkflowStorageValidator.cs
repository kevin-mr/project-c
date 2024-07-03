using FluentValidation;
using ProjectC.Server.Data.Entities;

namespace ProjectC.Server.Validators
{
    public class WorkflowStorageValidator : AbstractValidator<WorkflowStorage>
    {
        public WorkflowStorageValidator()
        {
            RuleFor(x => x.PropertyIdentifier).NotEmpty();
        }
    }
}
