using FluentValidation;
using MedicalAssistant.UI.Models.Validator;
using MedicalAssistant.UI.Shared.Resources;

namespace MedicalAssistant.UI.Models.Notifications;

public class VisitNotificationViewModelValidator : BaseValidator<VisitNotificationViewModel>
{
    public VisitNotificationViewModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;


        RuleFor(x => x.Date)
            .NotNull()
            .WithMessage(Translations.EmptyNotificationDate);


        RuleFor(x => x.Time)
            .NotNull()
            .WithMessage(Translations.EmptyNotificationDate);
    }
}