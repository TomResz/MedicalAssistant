using MedicalAssistant.UI.Shared.Resources;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.AppBar;

public partial class DeactivateAccountButton : ComponentBase
{
    [Inject] IDialogService DialogService { get; set; }
    private async Task ShowDialog()
    {
        var dialog = await DialogService.ShowAsync<DeactivateAccountDialog>(Translations.DeactivateAccount);
    }
}