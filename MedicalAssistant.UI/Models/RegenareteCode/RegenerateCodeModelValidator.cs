using FluentValidation;
using MedicalAssistant.UI.Models.Validator;

namespace MedicalAssistant.UI.Models.RegenareteCode;

public class RegenerateCodeModelValidator : BaseValidator<RegenerateCodeModel>
{
    public RegenerateCodeModelValidator()
    {
		RuleLevelCascadeMode = CascadeMode.Stop;
		RuleFor(x => x.Email)
            .EmailMustBeValid();
    }
}
