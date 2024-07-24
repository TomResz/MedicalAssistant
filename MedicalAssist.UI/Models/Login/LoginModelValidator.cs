using FluentValidation;
using MedicalAssist.UI.Models.Validator;

namespace MedicalAssist.UI.Models.Login;

public class LoginModelValidator : AbstractValidator<LoginModel>
{
	public LoginModelValidator()
	{
		RuleLevelCascadeMode = CascadeMode.Stop;
		RuleFor(x => x.Email).EmailMustBeValid();
		RuleFor(x => x.Password).PasswordMustBeValid();
	}


	public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
	{
		var result = await ValidateAsync(ValidationContext<LoginModel>.CreateWithOptions((LoginModel)model,
			x => x.IncludeProperties(propertyName)));
		if (result.IsValid)
			return Array.Empty<string>();
		return result.Errors.Select(e => e.ErrorMessage);
	};
}
