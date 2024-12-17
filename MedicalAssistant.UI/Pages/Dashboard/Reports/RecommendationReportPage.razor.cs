using MedicalAssistant.UI.Models.Medication;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Pages.Dashboard.Reports;

public partial class RecommendationReportPage
{
    [Inject] IMedicationService MedicationService { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
    [Inject] IReportService ReportService { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    
    private List<MedicationDto> _medications = [];
    private HashSet<MedicationDto> _selectedItems = [];

    private bool _loading = true;

    protected override async Task OnInitializedAsync()
    {
        var response = await MedicationService.GetAll();

        if (response.IsSuccess)
        {
            _medications = response.Value!;
        }

        _loading = false;
    }

    private void GoToDetails(Guid id) => NavigationManager.NavigateTo($"/medication/{id}");

    private async Task Download()
    {
        var ids = _selectedItems.Select(n => n.Id).ToList();

        if (ids.Count == 0)
        {
            return;
        }
        
        var isSuccess = await ReportService.DownloadMedicationReport(ids);

        if (!isSuccess)
        {
            Snackbar.Add(Translations.SomethingWentWrong, Severity.Error);
        }
    }
}