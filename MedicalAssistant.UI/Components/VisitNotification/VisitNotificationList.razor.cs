using MedicalAssistant.UI.Models.Notifications;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.VisitNotification;

public partial class VisitNotificationList
{
	[Parameter]
	public IReadOnlyList<VisitNotificationDto> Notifications { get; set; }

	[Parameter]
	public Guid VisitId { get; set; }

	[Inject]
	public IDialogService DialogService { get; set; }

	[Inject]
	public ILocalTimeProvider TimeProvider { get; set; }

	private List<VisitNotificationModel> _notificationModels = [];

	private bool _loading = true;

	protected override async Task OnInitializedAsync()
	{
		foreach (var notification in Notifications)
		{
			var localDate = await TimeProvider.FromUtcToLocal(notification.ScheduledDateUtc);
			_notificationModels.Add(new VisitNotificationModel
			{
				Id = notification.Id,
				VisitId = notification.VisitId,
				Date = localDate
			});
		}
		await InvokeAsync(StateHasChanged);
		_loading = false;
	}
	private async Task HandleEdit()
	{
		await InvokeAsync(StateHasChanged);	
	}
	private async Task HandleDelete(Guid id)
	{
		var notification = _notificationModels.First(x=>x.Id == id);
		_notificationModels.Remove(notification);
		await InvokeAsync(StateHasChanged);
	}

	private async Task Add()
	{

		var parameters = new DialogParameters
		{
			{ nameof(AddVisitNotificationItemDialog.VisitId), VisitId}
		};

		var options = new DialogOptions
		{
			CloseOnEscapeKey = true,
			FullWidth = true,
		};

		var dialog = DialogService.Show<AddVisitNotificationItemDialog>(
			Translations.AddNotification,
			parameters,
			options);

		var result = await dialog.Result;

		if(result is null)
		{
			return;
		}

		if(result.Data is VisitNotificationDto notification)
		{
			var localDate = await TimeProvider.FromUtcToLocal(notification.ScheduledDateUtc);
			_notificationModels.Add(new VisitNotificationModel
			{
				Id = notification.Id,
				VisitId = notification.VisitId,
				Date = localDate
			});
			_notificationModels = [.. _notificationModels.OrderBy(x => x.Date)];
			await InvokeAsync(StateHasChanged);
		}
	}


}
