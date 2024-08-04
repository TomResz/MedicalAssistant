using FluentValidation;
using MedicalAssist.UI.Models.Validator;

namespace MedicalAssist.UI.Models.PasswordChange;

public sealed class ForgotPasswordModelValidator : AbstractValidator<ForgotPasswordModel>
{
	public ForgotPasswordModelValidator()
	{
		RuleLevelCascadeMode = CascadeMode.Stop;
		RuleFor(x => x.Email)
			.EmailMustBeValid();
	}
	public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
	{
		var result = await ValidateAsync(ValidationContext<ForgotPasswordModel>.CreateWithOptions((ForgotPasswordModel)model,
			x => x.IncludeProperties(propertyName)));
		if (result.IsValid)
			return Array.Empty<string>();
		return result.Errors.Select(e => e.ErrorMessage);
	};
}