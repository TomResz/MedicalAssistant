using MedicalAssistant.UI.Models.Notifications;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.VisitNotification;

public partial class EditVisitNotificationItemDialog
{
	[Inject]
	public ILocalTimeProvider TimeProvider { get; set; }

    [Inject]
    public ISnackbar Snackbar { get; set; }

    [Inject]
	public IVisitNotificationService Service { get; set; }

	[CascadingParameter]
	public MudDialogInstance Dialog { get; set; }

	[Parameter]
	public DateTime Date { get; set; }

	[Parameter]
	public TimeSpan Time { get; set; }

	[Parameter]
	public Guid Id { get; set; }

	private MudForm _form;

	private readonly VisitNotificationViewModel _model = new();
	private readonly VisitNotificationViewModelValidator _validator = new();

	private bool _btnPressed = false;

	protected override void OnParametersSet()
	{
		_model.Date = Date;
		_model.Time = Time;
	}

	private async Task BtnPressed()
	{
		await _form.Validate();

		if (!_form.IsValid)
		{
			return;
		}
		var date = _model.Date!.Value.Add(_model.Time!.Value);
		var currentDate = await TimeProvider.CurrentDate();
		var dateUtc = await TimeProvider.FromLocalToUtc(date);


		var model = new EditVisitNotificationModel
		{
			Id = Id,
			CurrentDate = currentDate,
			DateUtc = dateUtc,
			Date = date,
		};

		Response response = await Service.ChangeDate(model);

		if (response.IsSuccess)
		{
			Dialog.Close(DialogResult.Ok(date));
			Snackbar.Add(Translations.NotificationEdited, Severity.Success);
		}
		else
		{
			var error = Service.MatchErrors(response.ErrorDetails!);
			Snackbar.Add(error, Severity.Error);
		}

	}
	private void Cancel() => Dialog.Cancel();
}
