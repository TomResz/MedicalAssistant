using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Response.Messages;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.Visits;

public partial class EditRemoveVisitDialog
{
	[Inject]
	public IDialogService DialogService { get; set; }

	[Inject]
	public IVisitService VisitService { get; set; }


	[CascadingParameter]
	private MudDialogInstance MudDialog { get; set; }

	[Parameter]
	public VisitDto VisitDto { get; set; }

	[Parameter]
	public EventCallback<Guid> OnVisitDeleted { get; set; }

	[Parameter]
	public EventCallback<VisitDto> OnVisitEdited { get; set; }

	[Inject]
	public ISnackbar Snackbar { get; set; }

	private bool _isEditMode = false;
	private bool _readOnlyMode => !_isEditMode;

	private MudForm form;
	private VisitViewModel _visitModel = new();
	private readonly VisitViewModelValidator _validator = new();

	private bool _btnPressed = false;

	protected override void OnInitialized()
	{
		_visitModel = VisitDto.ToViewModel();
	}

	private async Task BtnPressed()
	{
		if (!_isEditMode)
		{
			_isEditMode = true;
			return;
		}
		await form.Validate();
		if (!form.IsValid)
		{
			return;
		}
		_btnPressed = true;
		var editVisitModel = _visitModel.ToEditModel(VisitDto.Id);
		var response = await VisitService.Edit(editVisitModel);

		if (response.IsSuccess)
		{
			var dto = response.Value!;
			await OnVisitEdited.InvokeAsync(dto);
			MudDialog.Close();
		}
		else
		{
			var message = response.ErrorDetails!.Type switch
			{
				VisitErrorMessages.ConflictOfHours => Translations.VisitHoursConflict,
				_ => Translations.SomethingWentWrong
			};
			Snackbar.Add(message, Severity.Error);
		}
		_btnPressed = false;
	}



	public IMask postalCodeMask = new PatternMask("00-000");

	private void Cancel() => MudDialog.Cancel();


	#region Delete Visit

	private async Task DeleteVisit()
	{
		bool? result = await DialogService.ShowMessageBox(
			Translations.DialogVisitRemoving,
			(MarkupString)Translations.DialogVisitRemovingPrompt,
			yesText: Translations.DialogYes, cancelText: Translations.DialogNo);

		if (result is null || result is false)
		{
			return;
		}
		_btnPressed = true;
		var visitId = VisitDto.Id;
		var response = await VisitService.Delete(visitId);

		if (response.IsSuccess)
		{
			Snackbar.Add(message: Translations.VisitDeletedText, Severity.Info);
			await OnVisitDeleted.InvokeAsync(visitId);
			MudDialog.Close();
		}
		else
		{
			Snackbar.Add(Translations.SomethingWentWrong, Severity.Error);	
		}
		_btnPressed = false;
	}
	#endregion
}
