using MedicalAssistant.UI.Models.MedicalHistory;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;

namespace MedicalAssistant.UI.Pages.Dashboard.Reports;

public partial class DiseaseHistoryReportPage : ComponentBase
{
    [Inject] NavigationManager NavigationManager { get; set; }
    [Inject] IMedicalHistoryService VisitService { get; set; }
    [Inject] IReportService ReportService { get; set; }
    private bool _loading = true;
    private List<MedicalHistoryDto> _history = new();
    private HashSet<MedicalHistoryDto> _selectedItems = [];

    protected override async Task OnInitializedAsync()
    {
        var apiResponse = await VisitService.GetAll();
        if (apiResponse.IsSuccess)
        {
            _history = apiResponse.Value!;
        }
        _loading = false;
    }

    private async Task Download()
    {
        var ids = _selectedItems.Select(x => x.Id).ToList();
        await ReportService.DownloadMedicalHistoryReport(ids);
        
        //TODO 
        // SNACKBAR WITH MESSAGE IF API RESPONSE FAILED ( >= 400 CODE STATUS)
    }
    private void GoToDetails(Guid id) => NavigationManager.NavigateTo($"medical-history/{id}");
}