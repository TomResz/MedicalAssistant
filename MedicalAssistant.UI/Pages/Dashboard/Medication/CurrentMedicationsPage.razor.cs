using MedicalAssistant.UI.Components.Medication;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Pages.Dashboard.Medication;

public partial class CurrentMedicationsPage
{
	[Inject]
	public IDialogService DialogService { get; set; }

	private async Task AddRecommendationDialog()
	{
		var options = new DialogOptions()
		{
			MaxWidth = MaxWidth.ExtraLarge,

		};
		var dialog = await DialogService.ShowAsync<AddMedicationRecommendationDialog>("Title",options);
	}
}   
