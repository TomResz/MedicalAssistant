using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.Visits;

public partial class VisitDetails
{
	[Parameter]
	public VisitDto Visit { get; set; }

	[Inject]
	public NavigationManager NavigationManager { get; set; }

    [Inject]
    public IVisitService VisitService { get; set; }

    [Inject]
    public ISnackbar Snackbar { get; set; }

    [Inject]
	public IDialogService DialogService { get; set; }

	public TimeSpan PredictedTime => Visit.End - Visit.Date;

	private void ReturnToScheduler() => NavigationManager.NavigateTo("/visits");

	private void Edit()
	{
		var parameters = new DialogParameters
		{
			{ nameof(VisitDto), Visit },
			{ nameof(EditRemoveVisitDialog.OnVisitDeleted), EventCallback.Factory.Create<Guid>(this, OnVisitDeleted) },
			{ nameof(EditRemoveVisitDialog.OnVisitEdited), EventCallback.Factory.Create<VisitDto>(this, OnVisitEdited) }
		};
		var options = new MudBlazor.DialogOptions() { CloseOnEscapeKey = true, FullWidth = true };
		var dialog = DialogService.Show<EditRemoveVisitDialog>(Translations.Visit, parameters);
	}
	private async Task Delete()
	{
		bool? result = await DialogService.ShowMessageBox(
			Translations.DialogVisitRemoving,
			(MarkupString)Translations.DialogVisitRemovingPrompt,
			yesText: Translations.DialogYes, cancelText: Translations.DialogNo);

		if (result is null || result is false)
		{
			return;
		}
		var visitId = Visit.Id;
		var response = await VisitService.Delete(visitId);

		if (response.IsSuccess)
		{

			ReturnToScheduler();
			Snackbar.Add(Translations.VisitDeletedText, Severity.Info);
			return;
		}
		else
		{
			Snackbar.Add(Translations.SomethingWentWrong,Severity.Error);
		}
	}
	private void OnVisitDeleted(Guid visitId) => ReturnToScheduler();

	private async Task OnVisitEdited(VisitDto visitDto)
	{
		Visit = visitDto;
		await InvokeAsync(StateHasChanged);
	}

}
