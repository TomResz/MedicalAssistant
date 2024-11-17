using MedicalAssistant.UI.Models.MedicalHistory;
using Microsoft.AspNetCore.Components;

namespace MedicalAssistant.UI.Components.MedicalHistory;

public partial class MedicalHistoryDetails
{
    [Parameter] public MedicalHistoryDto MedicalHistory { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    private async Task Edit()
    {
        await Task.CompletedTask;
        // TO DO
    }

    private async Task Delete()
    {
        await Task.CompletedTask;
        // TO DO
    }

    private void ReturnToMainView()
    {
        NavigationManager.NavigateTo($"/medical-history");
    }
}