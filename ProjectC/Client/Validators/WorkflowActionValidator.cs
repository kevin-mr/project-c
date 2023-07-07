using FluentValidation;
using ProjectC.Shared.Models;

namespace ProjectC.Client.Validators
{
    public class WorkflowActionValidator : AbstractValidator<WorkflowActionDto>
    {
        public WorkflowActionValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue =>
            async (model, propertyName) =>
            {
                var result = await ValidateAsync(
                    ValidationContext<WorkflowActionDto>.CreateWithOptions(
                        (WorkflowActionDto)model,
                        x => x.IncludeProperties(propertyName)
                    )
                );
                return result.IsValid
                    ? (IEnumerable<string>)Array.Empty<string>()
                    : result.Errors.Select(e => e.ErrorMessage);
            };
    }
}
