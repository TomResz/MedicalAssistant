using MedicalAssistant.UI.Models.Medication;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Response.Base;
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
	public IMedicationService MedicationService { get; set; }

	[Inject]
	public IDialogService DialogService { get; set; }

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
		var selectedDate = args.Start;

		if (!validViews.Contains(args.View.Text))
		{
			return;
		}

		var dialogParameters = new DialogParameters
		{
			{nameof(AddMedicationRecommendationDialog.DateRange), new DateRange(selectedDate,selectedDate)},
		};

		var options = new MudBlazor.DialogOptions()
		{
			MaxWidth = MaxWidth.Large,
			FullWidth = true,
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
		var parameters = new DialogParameters
		{
			{ nameof(EditMedicationRecommendationDialog.Id),args.Data.Id},
			{ nameof(EditMedicationRecommendationDialog.Medication),args.Data},
			{nameof(EditMedicationRecommendationDialog.OnMedicationDeleted),EventCallback.Factory.Create<Guid>(this,MedicationDeleted)},
		};

		var options = new MudBlazor.DialogOptions() { CloseOnEscapeKey = true, FullWidth = true, MaxWidth = MaxWidth.Large };
		var dialog = DialogService.Show<EditMedicationRecommendationDialog>(Translations.Edit, parameters, options);
		var result = await dialog.Result;

		if (result is not null &&
			!result.Canceled &&
			result.Data is MedicationDto medication)
		{
			var index = _items.FindIndex(x => x.Id == medication.Id);

			if (index is not -1)
			{
				_items[index] = medication;
				await _scheduler.Reload();
			}
		}
	}

	public void MedicationDeleted(Guid medicationId)
	{
		var medication = _items.FirstOrDefault(x => x.Id == medicationId);
		
		if (medication is not null)
		{
			_items.Remove(medication);
			_scheduler.Reload();
		}
	}


	private async Task ShowMore(SchedulerMoreSelectEventArgs args)
	{
		var date = args.Start.Date;

		Response<List<MedicationDto>> response = await MedicationService.GetByDate(date);

		if (response.IsFailure || response.Value is null || response.Value.Count is 0)
		{
			return;
		}
		var medications = response.Value!;

		var parameters = new DialogParameters()
		{
			{nameof(MedicationSchedulerMoreSelectDialog.Medications),medications }
		};

		var options = new MudBlazor.DialogOptions()
		{
			FullWidth = true,
			MaxWidth = MaxWidth.Large,
			NoHeader = true,
		};

		var dialog = DialogService.Show<MedicationSchedulerMoreSelectDialog>(null, parameters, options);
	}
}
