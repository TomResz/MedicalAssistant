﻿using MedicalAssistant.UI.Models.Notifications;
using MedicalAssistant.UI.Shared.Options;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using MedicalAssistant.UI.Shared.Services.HubToken;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using MudBlazor;
using System.Text.Json;

namespace MedicalAssistant.UI.Components.AppBar.Notifications;

public partial class Notification : IAsyncDisposable
{
	[Inject]
	public INotificationService NotificationService { get; set; }
	[Inject]
	public NavigationManager NavigationManager { get; set; }

	[Inject]
	public IHubTokenService HubTokenService { get; set; }
	[Inject]
	public IOptions<APIOptions> APIOptions { get; set; }

	[Inject]
	public ISnackbar Snackbar { get; set; }

	private readonly List<NotificationModel> _models = new();
	private HubConnection? _hubConnection;
	private bool isExpanded = false;
	private bool isPopoverOpen = false;
	public int UnReadNotifications => _models.Where(x => x.WasRead == false).Count();


	protected override async Task OnInitializedAsync()
	{
		var uri = APIOptions.Value.NotificationHubUrl;
		_hubConnection = new HubConnectionBuilder()
				.WithUrl(uri,
					options => options.AccessTokenProvider = async () =>
					await HubTokenService.GetJwt())
				.WithAutomaticReconnect()
				.Build();

		var currentNotificationsResponse = await NotificationService.Get();

		if (currentNotificationsResponse.IsSuccess)
		{
			var results = currentNotificationsResponse.Value!;
			_models.AddRange(results);
			await InvokeAsync(StateHasChanged);
		}

		_hubConnection.On<NotificationModel>("ReceiveNotification", notification =>
		{
			_models.Add(notification);
			Snackbar.Add(Translations.NewNotifcation, MudBlazor.Severity.Normal);
			InvokeAsync(StateHasChanged);
		});

		await _hubConnection.StartAsync();
	}

	private void GoToNotificationDetails(NotificationModel notification)
		=> NotificationHelper.GoToNotificationDetails(notification, NavigationManager);

	public async ValueTask DisposeAsync()
	{
		if (_hubConnection is not null)
		{
			await _hubConnection.DisposeAsync();
		}
	}

	private void ToggleCollapse()
	{
		isExpanded = !isExpanded;
		isPopoverOpen = !isPopoverOpen;
	}

	#region Clear
	private async void ClearNotifications()
	{
		var ids = _models
			.Where(x => x.WasRead == false)
			.Select(x => x.Id).ToList();

		var response = await NotificationService.MarkAsRead(ids);

		if (response.IsSuccess)
		{
			var selectedNotifications = _models
				.Where(x => ids.Contains(x.Id))
				.ToList();

			selectedNotifications.ForEach(x => x.WasRead = true);
			await InvokeAsync(StateHasChanged);
		}

	}
	#endregion
}
