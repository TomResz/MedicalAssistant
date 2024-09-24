using FluentValidation;
using MedicalAssistant.UI.Models.Validator;

namespace MedicalAssistant.UI.Models.PasswordChange;

public sealed class ForgotPasswordModelValidator : BaseValidator<ForgotPasswordModel>
{
	public ForgotPasswordModelValidator()
	{
		RuleLevelCascadeMode = CascadeMode.Stop;
		RuleFor(x => x.Email)
			.EmailMustBeValid();
	}
}