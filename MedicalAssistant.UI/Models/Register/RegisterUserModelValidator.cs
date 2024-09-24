using FluentValidation;
using MedicalAssistant.UI.Models.Validator;
using MedicalAssistant.UI.Shared.Resources;

namespace MedicalAssistant.UI.Models.Register;

public sealed class RegisterUserModelValidator : BaseValidator<RegisterUserModel>
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
}
