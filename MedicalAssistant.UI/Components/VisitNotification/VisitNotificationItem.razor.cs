using MedicalAssistant.UI.Models.Notifications;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.VisitNotification;

public partial class VisitNotificationItem
{
    [Parameter]
    public VisitNotificationModel VisitNotification { get; set; }

    [Parameter]
    public string? Class { get; set; } = null;

    [Parameter]
    public EventCallback OnNotificationEdit { get; set; }

    [Parameter]
    public EventCallback<Guid> OnNotificationDelete { get; set; }

    [Inject]
    public IVisitNotificationService VisitNotificationService { get; set; }

    [Inject]
    public IDialogService DialogService { get; set; }

    [Inject]
    public ILocalTimeProvider LocalTimeProvider { get; set; }


	private MudMessageBox _mudMessageBox;

	private async Task Edit()
    {
        var parameters = new DialogParameters
        {
            { nameof(EditVisitNotificationItemDialog.Date), VisitNotification.Date.Date },
            { nameof(EditVisitNotificationItemDialog.Time), VisitNotification.Date.TimeOfDay },
            { nameof(EditVisitNotificationItemDialog.Id),VisitNotification.Id }
        };

        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
        };

        var dialog = DialogService.Show<EditVisitNotificationItemDialog>(
            Translations.EditNotification,
            parameters,
            options);

        var result = await dialog.Result;
        
        if(result is null)
        {
            return;
        }

        if(result.Data is DateTime date)
        {
            VisitNotification.Date = date;
            await OnNotificationEdit.InvokeAsync();
        }

    }

    private async Task Delete()
    {
        bool? dialogResult = await _mudMessageBox.ShowAsync();
        
        if(dialogResult is null || dialogResult == false)
        {
            return;
        }

        var response = await VisitNotificationService.Delete(VisitNotification.Id);

        if (response.IsSuccess)
        {
            await OnNotificationDelete.InvokeAsync(VisitNotification.Id);
        }
    }
}
