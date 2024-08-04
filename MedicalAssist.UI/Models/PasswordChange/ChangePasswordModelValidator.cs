using FluentValidation;
using MedicalAssist.UI.Models.Validator;
using MedicalAssist.UI.Shared.Resources;

namespace MedicalAssist.UI.Models.PasswordChange;

public class ChangePasswordModelValidator : AbstractValidator<ChangePasswordModel>
{

    public ChangePasswordModelValidator()
    {
		RuleLevelCascadeMode = CascadeMode.Stop;
		RuleFor(x => x.Password).PasswordMustBeValid();
		RuleFor(x => x.ConfirmedPassword)
			.Equal(x => x.Password)
			.WithMessage(Translations.PasswordMatch);
	}
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
	{
		var result = await ValidateAsync(ValidationContext<ChangePasswordModel>.CreateWithOptions((ChangePasswordModel)model,
			x => x.IncludeProperties(propertyName)));
		if (result.IsValid)
			return Array.Empty<string>();
		return result.Errors.Select(e => e.ErrorMessage);
	};
}