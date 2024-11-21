using FluentValidation;
using MedicalAssistant.UI.Components.MedicalHistory;
using MedicalAssistant.UI.Models.Validator;
using MedicalAssistant.UI.Shared.Resources;

namespace MedicalAssistant.UI.Models.MedicalHistory;

public class EditStageViewModelValidator : BaseValidator<EditStageViewModel>
{
    public EditStageViewModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotNull()
            .WithMessage(Translations.EmptyField)
            .NotEmpty()
            .WithMessage(Translations.EmptyField)
            .MaximumLength(30)
            .WithMessage(Translations.ExceededMaxSizeOfField);
    }
}