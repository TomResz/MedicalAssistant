using MedicalAssistant.UI.Models.Medication;
using MedicalAssistant.UI.Models.MedicationNotification;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.MedicationNotification;

public partial class AddMedicationNotificationDialog
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
	public Guid	MedicationId { get; set; }

	private DateRangeDto _dateRange = new();

	private DateRange DateRange { get; set; }
	private TimeSpan? Time { get; set; }	

	protected override async Task OnParametersSetAsync()
	{
		Time = DateTime.Now.TimeOfDay;
		var currentDate = (await LocalTimeProvider.CurrentDate()).Date;
		Response<DateRangeDto> response = await MedicationNotificationService.DateRange(MedicationId);
		
		if (response.IsSuccess)
		{
			_dateRange = response.Value!;

			if(currentDate > _dateRange.Start)
			{
				_dateRange.Start = currentDate;
			}

			DateRange = new(_dateRange.Start, _dateRange.End);
			_loading = false;
		}
		else
		{
			DialogInstance.Cancel();
			Snackbar.Add(Translations.SomethingWentWrong, Severity.Error);
		}
	}

	public async Task Add()
	{
		var offset = await LocalTimeProvider.TimeZoneOffset();
		var timeUtc = TimeOnly.FromTimeSpan((TimeSpan)Time!).AddHours(-offset);
		
		var request = new AddMedicationNotificationModel
		{
			MedicationRecommendationId = MedicationId,
			End = (DateTime)DateRange.End!,
			Start = (DateTime)DateRange.Start!,
			TriggerTimeUtc = timeUtc
		};

		var response = await MedicationNotificationService.Add(request);

		if (response.IsSuccess)
		{
			var id = response.Value!;
			var model = new MedicationNotificationWithDateRangeDto
			{
				Id = id,
				EndDate = (DateTime)DateRange.End!,
				StartDate = (DateTime)DateRange.Start!,
				MedicationId = MedicationId,
				Time = TimeOnly.FromTimeSpan((TimeSpan)Time!)
			};
			DialogInstance.Close(model	);
			return;
		}

	}
	public void Cancel() => DialogInstance.Cancel();
}
