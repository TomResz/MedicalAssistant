using MedicalAssistant.UI.Models.MedicalHistory;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;

namespace MedicalAssistant.UI.Pages.Dashboard.DiseaseHistory;

public partial class MedicalHistoryDetailsPage 
{
    [Parameter] public Guid Id { get; set; }
    [Inject] public IMedicalHistoryService MedicalHistoryService { get; set; }

    private MedicalHistoryDto _history = new();
    private bool _loading = true;

    protected override async Task OnInitializedAsync()
    {
        var response = await MedicalHistoryService.GetById(Id);
        if (response.IsSuccess)
        {
            _history = response.Value!;
            _loading = false;
        }
    }
}