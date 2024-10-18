using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Radzen;
using Radzen.Blazor;

namespace MedicalAssistant.UI.Components.Medication;

public partial class MedicationSchedulerComponent
{
	RadzenScheduler<MedicationDto> _scheduler;

	[Inject]
	public IDialogService DialogService { get; set; }

	[Inject]
	public ILocalTimeProvider LocalTimeProvider { get; set; }

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
		string[] validViews = [Translations.Month, Translations.Day, Translations.Week];

		if (!validViews.Contains(args.View.Text))
		{
			return;
		}

		var date = await LocalTimeProvider.CurrentDate();
		var dialogParameters = new DialogParameters
		{
			{nameof(AddMedicationRecommendationDialog.DateRange), new DateRange(date,date)},
		};

		var options = new MudBlazor.DialogOptions()
		{
			MaxWidth = MaxWidth.ExtraLarge,
		};

		var dialog = await DialogService.ShowAsync<AddMedicationRecommendationDialog>(
			Translations.AddingMedication,
			dialogParameters,
			options);

		var result = await dialog.Result;

		if (result is not null &&
			!result.Canceled &&
			result.Data is MedicationDto medication)
		{
			_items.Add(medication);
			await _scheduler.Reload();
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
	public VisitDto? Visit { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public string Name { get; set; }
	public string[] TimeOfDay { get; set; }
	public int Quantity { get; set; }
	public string? ExtraNote { get; set; }

}