using FluentValidation;
using MedicalAssist.UI.Models.Validator;
using MedicalAssist.UI.Shared.Resources;

namespace MedicalAssist.UI.Models.Register;

public class RegisterUserModelValidator : AbstractValidator<RegisterUserModel>
{
    public RegisterUserModelValidator()
	{
		RuleLevelCascadeMode = CascadeMode.Stop;

		RuleFor(x => x.Email).EmailMustBeValid();
		RuleFor(x => x.Password).PasswordMustBeValid();

		RuleFor(x => x.FullName)
			.NotEmpty()
			.WithMessage(Translations.EmptyFullname)
			.MinimumLength(3)
			.WithMessage(Translations.FullNameTooShort)
			.MaximumLength(100)
			.WithMessage(Translations.FullNameTooLong);

		RuleFor(x => x.ConfirmedPassword)
			.Equal(x => x.Password)
			.WithMessage(Translations.PasswordMatch);
    }

	public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
	{
		var result = await ValidateAsync(ValidationContext<RegisterUserModel>.CreateWithOptions((RegisterUserModel)model,
			x => x.IncludeProperties(propertyName)));
		if (result.IsValid)
			return Array.Empty<string>();
		return result.Errors.Select(e => e.ErrorMessage);
	};
}
