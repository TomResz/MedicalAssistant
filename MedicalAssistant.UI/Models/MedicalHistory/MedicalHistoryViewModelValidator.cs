using FluentValidation;
using MedicalAssistant.UI.Models.Validator;
using MedicalAssistant.UI.Shared.Resources;

namespace MedicalAssistant.UI.Models.MedicalHistory;

public class MedicalHistoryViewModelValidator : BaseValidator<MedicalHistoryViewModel>
{
    public MedicalHistoryViewModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotNull()
            .WithMessage(Translations.EmptyField)
            .NotEmpty()
            .WithMessage(Translations.EmptyField)
            .MaximumLength(100)
            .WithMessage(string.Format(Translations.ExceededMaxSizeOfField, 100));
    }
}