using FluentValidation;
using MedicalAssist.UI.Shared.Resources;

namespace MedicalAssist.UI.Models.Validator;

public static class PasswordExtensions
{
	public static IRuleBuilder<T, string> PasswordMustBeValid<T>(this IRuleBuilder<T, string> ruleBuilder)
		=> ruleBuilder.NotEmpty()
			 .WithMessage(Translations.Empty_Password)
			 .MinimumLength(8)
			 .WithMessage(string.Format(Translations.PasswordTooShort, 8))
			 .MaximumLength(200)
			 .WithMessage(string.Format(Translations.PasswordTooLong, 200));
}
