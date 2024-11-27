using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;

namespace MedicalAssistant.UI.Pages.Dashboard.Reports;

public partial class VisitReportPage : ComponentBase
{
    [Inject] NavigationManager NavigationManager { get; set; }
    [Inject] IVisitService VisitService { get; set; }
    [Inject] IReportService ReportService { get; set; }
    private bool _loading = true;
    private List<VisitDto> _visits = new();
    private HashSet<VisitDto> _selectedItems = [];

    protected override async Task OnInitializedAsync()
    {
        var apiResponse = await VisitService.GetAllVisits("asc");
        if (apiResponse.IsSuccess)
        {
            _visits = apiResponse.Value!;
        }
        _loading = false;
    }

    private async Task Download()
    {
        var ids = _selectedItems.Select(x => x.Id).ToList();
        await ReportService.DownloadVisitReport(ids);
        
        //TODO 
        // SNACKBAR WITH MESSAGE IF API RESPONSE FAILED ( >= 400 CODE STATUS)
    }
    private void GoToDetails(Guid id) => NavigationManager.NavigateTo($"visit/{id}");
}