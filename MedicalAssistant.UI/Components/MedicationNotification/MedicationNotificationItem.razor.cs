using MedicalAssistant.UI.Components.AppBar.Notifications;
using MedicalAssistant.UI.Models.MedicationNotification;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.MedicationNotification;

public partial class MedicationNotificationItem
{
	[Parameter]
	public MedicationNotificationWithDateRangeDto Notification { get; set; }

	[Parameter]
	public string Class { get; set; }

	[Parameter]
	public EventCallback<Guid> OnNotificationDeleted { get; set; }

	[Parameter]
	public EventCallback<MedicationNotificationWithDateRangeDto> OnNotificationEdited { get; set; }


	[Inject]
	public IMedicationNotificationService MedicationNotificationService { get; set; }

	[Inject]
	public IDialogService DialogService { get; set; }


	private MudMessageBox _mudMessageBox;

	public async Task Edit()
	{
		var parameters = new DialogParameters()
		{
			{nameof(EditMedicationNotificationDialog.MedicationId),Notification.MedicationId },
			{nameof(EditMedicationNotificationDialog.Notification),Notification }
		};
		var options = new DialogOptions()
		{
			FullWidth = false,
			MaxWidth = MaxWidth.Medium,
			BackdropClick = true,
		};

		var dialog = await DialogService.ShowAsync<EditMedicationNotificationDialog>(Translations.Edit, parameters, options);

		var response = await dialog.Result;

		if(response is not null &&
			!response.Canceled && 
			response.Data is MedicationNotificationWithDateRangeDto dto)
		{
			Console.WriteLine(dto.Time);
			await OnNotificationEdited.InvokeAsync(dto);
		}

	}

	public async Task Delete()
	{
		var result = await _mudMessageBox.ShowAsync();

		if (result is null ||
			result == false)
		{
			return;
		}

		Response response = await MedicationNotificationService.Delete(Notification.Id);

		if (response.IsSuccess)
		{
			await OnNotificationDeleted.InvokeAsync(Notification.Id);
		}
	}
}
