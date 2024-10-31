using MedicalAssistant.UI.Models.MedicationNotification;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.MedicationNotification;

public partial class MedicationNotificationList
{
	private bool _loading = false;
	private List<MedicationNotificationWithDateRangeDto> _notifications = [];


	[Parameter]
	public IReadOnlyList<MedicationNotificationWithDateRangeDto> Notifications { get; set; }

	[Parameter]
	public Guid MedicationId { get; set; }

	[Inject]
	public IDialogService DialogService { get; set; }

	[Inject]
	public ILocalTimeProvider TimeProvider { get; set; }

	[Inject]
	public ISnackbar Snackbar { get; set; }


	protected override void OnParametersSet()
	{
		_notifications = [.. Notifications];
		_loading = false;
	}

	public async Task Add()
	{
		var parameters = new DialogParameters()
		{
			{nameof(AddMedicationNotificationDialog.MedicationId),MedicationId }
		};
		var options = new DialogOptions()
		{
			FullWidth = false,
			MaxWidth = MaxWidth.Medium,
			BackdropClick = true,
		};

		var dialog = await DialogService.ShowAsync<AddMedicationNotificationDialog>(Translations.Add, parameters, options);
		
		var response = await dialog.Result;
		if(response is not null &&
			!response.Canceled &&
			response.Data is MedicationNotificationWithDateRangeDto dto)
		{
			_notifications.Add(dto);
			_notifications = [.. _notifications.OrderBy(x =>x.StartDate)];
			await InvokeAsync(StateHasChanged);
			Snackbar.Add(Translations.NotificationAdded,Severity.Success);
		}
	
	}

	public async Task NotificationEdit(MedicationNotificationWithDateRangeDto dto)
	{
		var index = _notifications.FindIndex(x=> x.Id == dto.Id);
		if (index is not -1)
		{
			_notifications[index] = dto;
			await InvokeAsync(StateHasChanged);
		}
	}

	public async Task NotificationDelete(Guid id)
	{
		var notification = _notifications.FirstOrDefault(x => x.Id == id);
		if (notification is not null)
		{
			_notifications.Remove(notification);
			await InvokeAsync(StateHasChanged);
		}
	}
}
