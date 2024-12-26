using MedicalAssistant.UI.Models.Notifications;
using MedicalAssistant.UI.Shared.Options;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using MudBlazor;

namespace MedicalAssistant.UI.Components.AppBar.Notifications;

public partial class Notification : IAsyncDisposable
{
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    [Inject] public IHubTokenService HubTokenService { get; set; }
    [Inject] public IOptions<APIOptions> APIOptions { get; set; }

    [Inject] public ISnackbar Snackbar { get; set; }

    [Inject] public IJSRuntime JSRuntime { get; set; }

    private readonly List<NotificationModel> _models = [];
    private HubConnection? _hubConnection;
    private bool _isExpanded = false;
    private bool _isPopoverOpen = false;
    private bool _isSmallScreen = false;
    private int UnReadNotifications => _models.Count(x => x.WasRead == false);

    private bool _isInitialized = false;

    protected override async Task OnInitializedAsync()
    {
        // var uri = APIOptions.Value.NotificationHubUrl;
        // _hubConnection = new HubConnectionBuilder()
        // 		.WithUrl(uri,
        // 			options => options.AccessTokenProvider = async () =>
        // 			await HubTokenService.GetJwt())
        // 		.WithAutomaticReconnect()
        // 		.Build();
        //
        // var currentNotificationsResponse = await NotificationService.Get();
        //
        // if (currentNotificationsResponse.IsSuccess)
        // {
        // 	var results = currentNotificationsResponse.Value!;
        // 	_models.AddRange(results);
        // 	if (UnReadNotifications > 0)
        // 	{
        // 		await JSRuntime.InvokeVoidAsync("addNotificationTitlePrefix", UnReadNotifications);
        // 	}
        // 	await InvokeAsync(StateHasChanged);
        // }
        //
        // _hubConnection.On<NotificationModel>("ReceiveNotification", async notification =>
        // {
        // 	_models.Add(notification);
        // 	Snackbar.Add(Translations.NewNotifcation, MudBlazor.Severity.Normal);
        // 	if (UnReadNotifications > 0)
        // 	{
        // 		await JSRuntime.InvokeVoidAsync("addNotificationTitlePrefix", UnReadNotifications);
        // 	}
        // 	await InvokeAsync(StateHasChanged);
        // });
        //
        // await _hubConnection.StartAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!_isInitialized)
        {
            _isInitialized = true;

            var uri = APIOptions.Value.NotificationHubUrl;
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(uri,
                    options =>
                    {
                        options.AccessTokenProvider = async () =>
                            await HubTokenService.GetJwt();
                    })
                .WithAutomaticReconnect()
                .Build();

            var currentNotificationsResponse = await NotificationService.Get();

            if (currentNotificationsResponse.IsSuccess)
            {
                var results = currentNotificationsResponse.Value!;
                _models.AddRange(results);
                if (UnReadNotifications > 0)
                {
                    await JSRuntime.InvokeVoidAsync("addNotificationTitlePrefix", UnReadNotifications);
                }

                await InvokeAsync(StateHasChanged);
            }

            _hubConnection.On<NotificationModel>("ReceiveNotification", async notification =>
            {
                _models.Add(notification);
                Snackbar.Add(Translations.NewNotifcation, MudBlazor.Severity.Normal);
                if (UnReadNotifications > 0)
                {
                    await JSRuntime.InvokeVoidAsync("addNotificationTitlePrefix", UnReadNotifications);
                }

                await InvokeAsync(StateHasChanged);
            });

            await _hubConnection.StartAsync();
        }
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
        _isExpanded = !_isExpanded;
        _isPopoverOpen = !_isPopoverOpen;
    }

    #region Clear

    private async void ClearNotifications()
    {
        var ids = _models
            .Where(x => x.WasRead == false)
            .Select(x => x.Id).ToList();

        if (ids.Count == 0)
        {
            return;
        }

        var response = await NotificationService.MarkAsRead(ids);

        if (response.IsSuccess)
        {
            var selectedNotifications = _models
                .Where(x => ids.Contains(x.Id))
                .ToList();

            selectedNotifications.ForEach(x => x.WasRead = true);
            await JSRuntime.InvokeVoidAsync("removePrefix");
            await InvokeAsync(StateHasChanged);
        }
    }

    #endregion
}