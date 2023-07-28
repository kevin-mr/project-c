using FluentValidation;
using ProjectC.Shared.Models;

namespace ProjectC.Client.Validators
{
    public class WorkflowStorageDtoValidator : AbstractValidator<WorkflowStorageDto>
    {
        public WorkflowStorageDtoValidator()
        {
            RuleFor(x => x.PropertyIdentifier).NotEmpty();
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue =>
            async (model, propertyName) =>
            {
                var result = await ValidateAsync(
                    ValidationContext<WorkflowStorageDto>.CreateWithOptions(
                        (WorkflowStorageDto)model,
                        x => x.IncludeProperties(propertyName)
                    )
                );
                return result.IsValid
                    ? (IEnumerable<string>)Array.Empty<string>()
                    : result.Errors.Select(e => e.ErrorMessage);
            };
    }
}
