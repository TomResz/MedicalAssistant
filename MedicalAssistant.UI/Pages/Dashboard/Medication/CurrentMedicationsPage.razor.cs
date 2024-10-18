using MedicalAssistant.UI.Components.Medication;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Pages.Dashboard.Medication;

public partial class CurrentMedicationsPage
{
	[Inject]
	public IDialogService DialogService { get; set; }

	[Inject]
	public ILocalTimeProvider LocalTimeProvider { get; set; }

	private async Task AddRecommendationDialog()
	{
		var currentTime = await LocalTimeProvider.CurrentDate();

		var parameters = new DialogParameters()
		{
			{nameof(AddMedicationRecommendationDialog.DateRange),new DateRange(currentTime,currentTime) },

		};

		var options = new DialogOptions()
		{
			MaxWidth = MaxWidth.ExtraLarge,

		};

		var dialog = await DialogService.ShowAsync<AddMedicationRecommendationDialog>(
				Translations.AddingMedication, 
				parameters,
				options);
	}
}
