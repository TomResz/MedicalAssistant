using MedicalAssistant.UI.Models.Medication;
using MedicalAssistant.UI.Shared.Resources;
using Microsoft.AspNetCore.Components;

namespace MedicalAssistant.UI.Components.Medication;

public partial class MedicationWeekList
{
	[Parameter]
	public string ListName { get; set; }

	[Parameter]
	public IReadOnlyList<MedicationWithDayDto> Medications { get; set; }

	private Dictionary<DayOfWeek, List<MedicationWithDayDto>> GroupedMedications { get; set; }

	private readonly IReadOnlyList<DayOfWeek> _days =
	[
		DayOfWeek.Monday,
		DayOfWeek.Tuesday,
		DayOfWeek.Wednesday,
		DayOfWeek.Thursday,
		DayOfWeek.Friday,
		DayOfWeek.Saturday,
		DayOfWeek.Sunday
	];


	private string DayOfWeekToString(DayOfWeek day)
		=> day switch
		{
			DayOfWeek.Monday => Translations.Monday,
			DayOfWeek.Tuesday => Translations.Tuesday,
			DayOfWeek.Wednesday => Translations.Wednesday,
			DayOfWeek.Thursday => Translations.Thursday,
			DayOfWeek.Friday => Translations.Friday,
			DayOfWeek.Saturday => Translations.Saturday,
			_ => Translations.Sunday
		};

	protected override Task OnInitializedAsync()
	{
		GroupedMedications = [];

		foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
		{
			GroupedMedications[day] = [];
		}

		foreach (var medication in Medications)
		{
			DayOfWeek dayOfWeek = medication.Day.DayOfWeek;
			GroupedMedications[dayOfWeek].Add(medication);
		}

		return Task.CompletedTask;
	}
}
