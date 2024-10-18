using FluentValidation;
using MedicalAssistant.UI.Models.Validator;
using MedicalAssistant.UI.Shared.Resources;
using MudBlazor;

namespace MedicalAssistant.UI.Models.Medication;

public class MedicationViewModel
{
	public string MedicineName { get; set; }
	public int Quantity { get; set; } = 1;
	public bool MorningChecked { get; set; }
	public bool EveningChecked { get; set; }
	public bool AfternoonChecked { get; set; }
	public bool NightChecked { get; set; }
	public Guid? VisitId { get; set; }
	public DateRange DateRange { get; set; }
	public string? ExtraNote { get; set; }
}

public class MedicationViewModelValidator : BaseValidator<MedicationViewModel>
{
    public MedicationViewModelValidator()
    {
		RuleLevelCascadeMode = CascadeMode.Stop;

		RuleFor(x => x.MedicineName)
			.NotEmpty()
			.WithMessage(Translations.MedicineNameNotEmpty);

		RuleFor(x => x.NightChecked)
			.Custom((nightChecked, ctx) =>
			{
				var model = ctx.InstanceToValidate;
				if (!model.MorningChecked && !model.AfternoonChecked && !model.EveningChecked && !nightChecked)
				{
					ctx.AddFailure("NightChecked", Translations.InvalidTimeOfDay);
				}
			});
	}
}