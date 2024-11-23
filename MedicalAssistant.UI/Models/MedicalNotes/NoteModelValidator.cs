using FluentValidation;
using MedicalAssistant.UI.Models.Validator;
using MedicalAssistant.UI.Shared.Resources;

namespace MedicalAssistant.UI.Models.MedicalNotes;

public class NoteModelValidator : BaseValidator<NoteModel>
{
    public NoteModelValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(x=>x.Note)
            .NotEmpty()
            .WithMessage(string.Format(Translations.ExceededMaxSizeOfField,15));

        RuleFor(x => x.CurrentTag)
            .Must((x,model) => x.Tags != null && x.Tags.Count != 0)
            .WithMessage(Translations.TagsCannotBeEmpty);

    }
}