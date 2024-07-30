using FluentValidation;
using MedicalAssist.UI.Models.Validator;

namespace MedicalAssist.UI.Models.RegenareteCode;

public class RegenerateCodeModelValidator : AbstractValidator<RegenerateCodeModel>
{
    public RegenerateCodeModelValidator()
    {
		RuleLevelCascadeMode = CascadeMode.Stop;
		RuleFor(x => x.Email)
            .EmailMustBeValid();
    }
	public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
	{
		var result = await ValidateAsync(ValidationContext<RegenerateCodeModel>.CreateWithOptions((RegenerateCodeModel)model,
			x => x.IncludeProperties(propertyName)));
		if (result.IsValid)
			return Array.Empty<string>();
		return result.Errors.Select(e => e.ErrorMessage);
	};
}
