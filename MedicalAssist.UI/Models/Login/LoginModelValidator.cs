using FluentValidation;

namespace MedicalAssist.UI.Models.Login;

public class LoginModelValidator : AbstractValidator<LoginModel>
{
    public LoginModelValidator()
    {
		RuleFor(x => x.Email)
			 .NotEmpty()
			 .WithMessage("Email nie może być pusty!");
		RuleFor(x => x.Password)
			.NotEmpty()
			 .WithMessage("Hasło nie może być puste!");
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
