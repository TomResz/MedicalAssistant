using FluentValidation;
using MedicalAssistant.UI.Models.Validator;

namespace MedicalAssistant.UI.Models.Login;

public class LoginModelValidator : BaseValidator<LoginModel>
{
	public LoginModelValidator()
	{
		RuleLevelCascadeMode = CascadeMode.Stop;
		RuleFor(x => x.Email).EmailMustBeValid();
		RuleFor(x => x.Password).PasswordMustBeValid();
	}
}
