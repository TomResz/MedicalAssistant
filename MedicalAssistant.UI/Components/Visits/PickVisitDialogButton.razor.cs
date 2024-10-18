using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.Visits;

public partial class PickVisitDialogButton
{
	[Inject]
	public IDialogService DialogService { get; set; }

	[Inject]
	public IVisitService VisitService { get; set; }

	[Inject]
	public ILocalTimeProvider LocalTimeProvider { get; set; }

	[Inject]
	public ISnackbar Snackbar { get; set; }

	[Parameter]
	public string ButtonName { get; set; }

	[Parameter]
	public string? Class { get; set; } = null;

	[Parameter]
	public EventCallback<VisitDto> OnVisitPicked { get; set; }

    public VisitDto? SelectedVisit = null;

	public async Task OpenDialog()
	{
		var currentDate = await LocalTimeProvider.CurrentDate();
		var response = await VisitService.GetCompleted(currentDate);

		if (response.IsSuccess)
		{
			var visits = response.Value!;

			if(visits is null
				|| visits.Count == 0)
			{
				Snackbar.Add(Translations.NoVisitsToChoose, Severity.Warning);
				return;
			}


			var visitId = SelectedVisit?.Id;

			DialogParameters parameters = new()
			{
				{nameof(SelectVisitDialog.Visits),visits},
				{nameof(SelectVisitDialog.SelectedId),visitId }
			};

			DialogOptions options = new DialogOptions()
			{
				CloseOnEscapeKey = true,
				FullWidth = true,
				MaxWidth = MaxWidth.ExtraExtraLarge,
			};

			var dialog = DialogService.Show<SelectVisitDialog>(Translations.SelectVisit, parameters, options);

			var resultDialog = await dialog.Result;

			if (resultDialog is not null
				&& !resultDialog.Canceled
				&& resultDialog.Data is VisitDto visit)
			{
				SelectedVisit = visit;
				await OnVisitPicked.InvokeAsync(visit);
			}
		}
	}
}
