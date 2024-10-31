using MedicalAssistant.UI.Models.Medication;
using MedicalAssistant.UI.Models.MedicationNotification;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.MedicationNotification;

public partial class EditMedicationNotificationDialog
{
	private bool _btnPressed = false;
	private bool _loading = true;

	[CascadingParameter]
	public MudDialogInstance DialogInstance { get; set; }

	[Inject]
	public IMedicationNotificationService MedicationNotificationService { get; set; }

	[Inject]
	public ISnackbar Snackbar { get; set; }

	[Inject]
	public ILocalTimeProvider LocalTimeProvider { get; set; }

	[Parameter]
	public Guid MedicationId { get; set; }

	[Parameter]
	public MedicationNotificationWithDateRangeDto Notification { get; set; }

	private DateRange DateRange { get; set; }
	private TimeSpan? Time { get; set; }

	private DateRangeDto _dateRange = new();

	protected override void OnParametersSet()
	{
		_dateRange.Start = Notification.StartDate;
		_dateRange.End = Notification.EndDate;
		DateRange = new(_dateRange.Start, _dateRange.End);
		Time = Notification.Time.ToTimeSpan();

	}


	private async Task Edit()
	{
		_btnPressed = true;
		var offset = await LocalTimeProvider.TimeZoneOffset();
		
		var request = new EditMedicationNotificationModel
		{
			Id = Notification.Id,
			Start = (DateTime)DateRange.Start!,
			End = (DateTime)DateRange.End!,
			TriggerTimeUtc = TimeOnly.FromTimeSpan((TimeSpan)Time!).AddHours(-offset),
		};

		var response = await MedicationNotificationService.Edit(request);

		if (response.IsSuccess)
		{
			var dto = new MedicationNotificationWithDateRangeDto
			{
				Id = Notification.Id ,
				StartDate = (DateTime)DateRange.Start!,
				EndDate = (DateTime)DateRange.End!,
				Time = TimeOnly.FromTimeSpan((TimeSpan)Time!),
				MedicationId = MedicationId,
			};
			DialogInstance.Close(dto);
			Snackbar.Add(Translations.NotificationEdited,Severity.Success);
		}
		_btnPressed = false;
	}
	private void Cancel() => DialogInstance.Cancel();

}
