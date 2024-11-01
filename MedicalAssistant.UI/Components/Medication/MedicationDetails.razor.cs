using MedicalAssistant.UI.Models.Medication;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.Medication;

public partial class MedicationDetails
{
	[Inject]
	public NavigationManager Navigation { get; set; }

	[Inject]
	public IDialogService DialogService { get; set; }

	[Inject]
	public IMedicationService MedicationService { get; set; }

	[Inject]
	public ISnackbar Snackbar { get; set; }

	[Parameter]
	public MedicationDto Medication { get; set; }

	private async Task Edit()
	{
		var parameters = new DialogParameters
		{
			{ nameof(EditMedicationRecommendationDialog.Id),Medication.Id},
			{ nameof(EditMedicationRecommendationDialog.Medication),Medication},
			{nameof(EditMedicationRecommendationDialog.OnMedicationDeleted),EventCallback.Factory.Create<Guid>(this,ReturnToScheduler)},
		};

		var options = new MudBlazor.DialogOptions() { CloseOnEscapeKey = true, FullWidth = true, MaxWidth = MaxWidth.Large };
		var dialog = DialogService.Show<EditMedicationRecommendationDialog>(Translations.Edit, parameters, options);
		var result = await dialog.Result;

		if (result is not null &&
			!result.Canceled &&
			result.Data is MedicationDto medication)
		{
			Medication = medication;
			await InvokeAsync(StateHasChanged);
		}
	}

	private async Task Delete()
	{
		bool? result = await DialogService.ShowMessageBox(
			Translations.DialogVisitRemoving,
			(MarkupString)Translations.DialogRemoveMedication,
			yesText: Translations.DialogYes, cancelText: Translations.DialogNo);

		if (result is null || result == false)
		{
			return;
		}

		var response = await MedicationService.Delete(Medication.Id);

		if (response.IsSuccess)
		{
			ReturnToScheduler();
			Snackbar.Add(Translations.MedicationDeleted, Severity.Normal);
		}
	}

	private void ReturnToScheduler() => Navigation.NavigateTo("/medication");
}
