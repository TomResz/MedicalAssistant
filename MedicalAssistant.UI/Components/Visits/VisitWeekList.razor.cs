using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Resources;
using Microsoft.AspNetCore.Components;

namespace MedicalAssistant.UI.Components.Visits;

public partial class VisitWeekList
{
	[Parameter]
	public string ListName { get; set; }

	[Parameter]
	public IReadOnlyList<VisitDto> Visits { get; set; }
	
	[Parameter]
	public DateTime DateOfMonday { get; set; }
	
	[Parameter]
	public DateTime DateOfSunday { get; set; }
	
	private Dictionary<DayOfWeek, List<VisitDto>> GroupedVisits { get; set; }

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
		GroupedVisits = [];

		foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
		{
			GroupedVisits[day] = [];
		}

		foreach (var visit in Visits)
		{
			DayOfWeek dayOfWeek = visit.Date.DayOfWeek;
			GroupedVisits[dayOfWeek].Add(visit);
		}

		return Task.CompletedTask;
	}
}
