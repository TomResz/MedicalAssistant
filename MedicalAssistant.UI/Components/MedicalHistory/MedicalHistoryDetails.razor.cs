using MedicalAssistant.UI.Models.MedicalHistory;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.MedicalHistory;

public partial class MedicalHistoryDetails
{
    [Parameter] public MedicalHistoryDto MedicalHistory { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IMedicalHistoryService MedicalHistoryService { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }

    private async Task Edit()
    {
        var parameters = new DialogParameters()
        {
            { nameof(EditMedicalHistoryDialog.MedicalHistory), MedicalHistory }
        };

        var options = new DialogOptions()
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
        };

        var dialog = await DialogService.ShowAsync<EditMedicalHistoryDialog>(
            Translations.EditMedicalHistory,
            parameters,
            options);
        
        var result = await dialog.Result;

        if (result is not null &&
            result is { Canceled: false, Data: MedicalHistoryDto response })
        {
            MedicalHistory = response;
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task Delete()
    {
        bool? result = await DialogService.ShowMessageBox(
            Translations.DialogMedicalHistoryDeleteConfirmation,
            (MarkupString)Translations.DialogRemoveMedicalHistory,
            yesText: Translations.DialogYes, cancelText: Translations.DialogNo);

        if (result is null or false)
        {
            return;
        }

        var response = await MedicalHistoryService.Delete(MedicalHistory.Id);

        if (response.IsSuccess)
        {
            NavigationManager.NavigateTo("/medical-history");
            Snackbar.Add(Translations.MedicalHistoryDeleted, Severity.Info);
            return;
        }

        Snackbar.Add(Translations.SomethingWentWrong, Severity.Error);
    }

    private void ReturnToMainView()
        => NavigationManager.NavigateTo($"/medical-history");
}