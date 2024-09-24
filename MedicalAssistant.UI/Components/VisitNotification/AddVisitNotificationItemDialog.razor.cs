using MedicalAssistant.UI.Models.Notifications;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.VisitNotification;

public partial class AddVisitNotificationItemDialog
{
	[Inject]
	public ILocalTimeProvider TimeProvider { get; set; }

	[Inject]
	public IVisitNotificationService Service { get; set; }

	[Inject]
	public ISnackbar Snackbar { get; set; }

	[CascadingParameter]
	public MudDialogInstance Dialog { get; set; }

	[Parameter]
	public Guid VisitId { get; set; }

	private MudForm _form;

	private readonly VisitNotificationViewModel _model = new();
	private readonly VisitNotificationViewModelValidator _validator = new();

	private bool _btnPressed = false;

	private async Task BtnPressed()
	{
		await _form.Validate();

		if (!_form.IsValid)
		{
			return;
		}

		_btnPressed = true;

		var date = _model.Date!.Value.Add(_model.Time!.Value);
		var currentDate = await TimeProvider.CurrentDate();
		var dateUtc = await TimeProvider.FromLocalToUtc(date);


		var model = new AddVisitNotificationModel
		{
			VisitId = VisitId,
			CurrentDate = currentDate,
			ScheduledDateUtc = dateUtc,
			ScheduledDate = date,
		};

		Response<VisitNotificationDto> response = await Service.Add(model);

		if (response.IsSuccess)
		{
			Dialog.Close(DialogResult.Ok(response.Value!));
			Snackbar.Add(Translations.NotificationAdded, Severity.Success);
			return;	
		}
		else
		{
			var error = Service.MatchErrors(response.ErrorDetails!);
			Snackbar.Add(error,Severity.Error);
		}
		_btnPressed = false;

	}
	private void Cancel() => Dialog.Cancel();
}

public class AddVisitNotificationModel
{
	public Guid VisitId { get; set; }
	public DateTime ScheduledDate { get; set; }
	public DateTime ScheduledDateUtc { get; set; }
	public DateTime CurrentDate { get; set; }
}
