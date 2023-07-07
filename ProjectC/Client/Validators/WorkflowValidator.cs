using FluentValidation;
using ProjectC.Shared.Models;

namespace ProjectC.Client.Validators
{
    public class WorkflowValidator : AbstractValidator<WorkflowDto>
    {
        public WorkflowValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue =>
            async (model, propertyName) =>
            {
                var result = await ValidateAsync(
                    ValidationContext<WorkflowDto>.CreateWithOptions(
                        (WorkflowDto)model,
                        x => x.IncludeProperties(propertyName)
                    )
                );
                return result.IsValid
                    ? (IEnumerable<string>)Array.Empty<string>()
                    : result.Errors.Select(e => e.ErrorMessage);
            };
    }
}
