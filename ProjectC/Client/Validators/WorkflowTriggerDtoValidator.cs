using FluentValidation;
using ProjectC.Shared.Models;

namespace ProjectC.Client.Validators
{
    public class WorkflowTriggerDtoValidator : AbstractValidator<WorkflowTriggerDto>
    {
        public WorkflowTriggerDtoValidator()
        {
            RuleFor(x => x.Description).NotEmpty();
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue =>
            async (model, propertyName) =>
            {
                var result = await ValidateAsync(
                    ValidationContext<WorkflowTriggerDto>.CreateWithOptions(
                        (WorkflowTriggerDto)model,
                        x => x.IncludeProperties(propertyName)
                    )
                );
                return result.IsValid
                    ? (IEnumerable<string>)Array.Empty<string>()
                    : result.Errors.Select(e => e.ErrorMessage);
            };
    }
}
