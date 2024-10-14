using MedicalAssistant.UI.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace MedicalAssistant.UI.Components.Medication;

public partial class MedicationSchedulerComponent
{
	RadzenScheduler<MedicationDto> _scheduler;

	[Parameter]
	public IReadOnlyList<MedicationDto> Items { get; set; }

	private List<MedicationDto> _items = [];

	protected override async Task OnInitializedAsync()
	{
		_items = [.. Items];
		await InvokeAsync(StateHasChanged);
	}


	private void SelectTodayDate(SchedulerSlotRenderEventArgs args)
	{
		string[] validViews = [Translations.Month, Translations.Year];
		if (validViews.Contains(args.View.Text) && args.Start.Date == DateTime.Today)
		{
			args.Attributes["style"] = "background: var(--rz-scheduler-highlight-background-color, rgba(255,220,40,.2));";
		}
	}

	private async Task AddMedication(SchedulerSlotSelectEventArgs args)
	{
		await Task.CompletedTask;
		string[] validViews = [Translations.Month, Translations.Day, Translations.Week];

		if (!validViews.Contains(args.View.Text))
		{
			return;
		}
	}

	private async Task EditMedication(SchedulerAppointmentSelectEventArgs<MedicationDto> args)
	{
		await Task.CompletedTask;
	}
}

public class MedicationDto
{
	public Guid Id { get; set; }
	public DateTime Start { get; set; }
	public DateTime End { get; set; }
	public string Name { get; set; }
	public string? Note { get; set; }
	public int Dosage { get; set; }
}