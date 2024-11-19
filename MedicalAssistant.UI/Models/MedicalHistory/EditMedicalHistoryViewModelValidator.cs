using FluentValidation;
using MedicalAssistant.UI.Models.Validator;
using MedicalAssistant.UI.Shared.Resources;

namespace MedicalAssistant.UI.Models.MedicalHistory;

public class EditMedicalHistoryViewModelValidator : BaseValidator<EditMedicalHistoryViewModel>
{
    public EditMedicalHistoryViewModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name)
            .NotNull()
            .WithMessage(Translations.EmptyField)
            .NotEmpty()
            .WithMessage(Translations.EmptyField)
            .MaximumLength(30)
            .WithMessage(Translations.ExceededMaxSizeOfField);

        RuleFor(x => x.EndDate)
            .Must((x, obj) =>
            {
                if (obj.HasValue)
                {
                    return obj.Value.Date >= x.StartDate!.Value.Date;
                }

                return true;
            }).WithMessage(Translations.EndDateMustBeGreater);
    }
}