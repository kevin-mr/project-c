using FluentValidation;
using ProjectC.Shared.Models;

namespace ProjectC.Client.Validators
{
    public class RequestRuleTriggerDtoValidator : AbstractValidator<RequestRuleTriggerDto>
    {
        public RequestRuleTriggerDtoValidator()
        {
            RuleFor(x => x.Description).NotEmpty();
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue =>
            async (model, propertyName) =>
            {
                var result = await ValidateAsync(
                    ValidationContext<RequestRuleTriggerDto>.CreateWithOptions(
                        (RequestRuleTriggerDto)model,
                        x => x.IncludeProperties(propertyName)
                    )
                );
                return result.IsValid
                    ? (IEnumerable<string>)Array.Empty<string>()
                    : result.Errors.Select(e => e.ErrorMessage);
            };
    }
}
