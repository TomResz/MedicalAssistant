using FluentValidation;
using MedicalAssistant.UI.Models.Validator;
using MedicalAssistant.UI.Shared.Resources;

namespace MedicalAssistant.UI.Models.PasswordChange;

public class ChangePasswordModelValidator : BaseValidator<ChangePasswordModel>
{

    public ChangePasswordModelValidator()
    {
		RuleLevelCascadeMode = CascadeMode.Stop;
		RuleFor(x => x.Password).PasswordMustBeValid();
		RuleFor(x => x.ConfirmedPassword)
			.Equal(x => x.Password)
			.WithMessage(Translations.PasswordMatch);
	}

}