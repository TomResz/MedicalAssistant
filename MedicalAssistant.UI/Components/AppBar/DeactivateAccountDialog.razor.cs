using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using MedicalAssistant.UI.Shared.Services.Auth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace MedicalAssistant.UI.Components.AppBar;

public partial class DeactivateAccountDialog
{
    [CascadingParameter] MudDialogInstance Dialog { get; set; }
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] IUserAuthService UserAuthService { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    [Inject] IDialogService DialogService { get; set; }
    private async Task DeleteAccount()
    {
        bool? result = await DialogService.ShowMessageBox(
            Translations.DeleteAccount,
            (MarkupString)Translations.AccountDeletePrompt,
            yesText: Translations.DialogYes, cancelText: Translations.DialogNo);

        if (result is null or false)
        {
            return;
        }
        
        Response response = await UserAuthService.DeleteAccount();

        if (response.IsSuccess)
        {
            Dialog.Close();
            await (AuthenticationStateProvider as MedicalAssistantAuthenticationStateProvider)!.LogOutAsync();
            await InvokeAsync(StateHasChanged);
            Snackbar.Add(Translations.AccountDeletedSuccessfully, Severity.Info);
            return;
        }

        Snackbar.Add(Translations.SomethingWentWrong, Severity.Error);
        
    }
    private async Task DeactivateAccount()
    {
        bool? result = await DialogService.ShowMessageBox(
            Translations.DeactivateAccount,
            (MarkupString)Translations.AccountDeactivationPrompt,
            yesText: Translations.DialogYes, cancelText: Translations.DialogNo);

        if (result is null or false)
        {
            return;
        }
        
        Response response = await UserAuthService.DeactivateAccount();

        if (response.IsSuccess)
        {
            Dialog.Close();
            await (AuthenticationStateProvider as MedicalAssistantAuthenticationStateProvider)!.LogOutAsync();
            await InvokeAsync(StateHasChanged);
            Snackbar.Add(Translations.AccountDeactivatedSuccessfully, Severity.Info);
            return;
        }

        Snackbar.Add(Translations.SomethingWentWrong, Severity.Error);
    }

    private void Cancel() => Dialog.Cancel();
}