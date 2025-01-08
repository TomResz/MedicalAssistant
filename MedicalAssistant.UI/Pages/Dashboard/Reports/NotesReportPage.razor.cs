using MedicalAssistant.UI.Models.MedicalNotes;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Pages.Dashboard.Reports;

public partial class NotesReportPage
{
    [Inject]
    private IMedicalNoteService NoteService { get; set; }
    
    [Inject]
    private IReportService ReportService { get; set; }
    
    [Inject]
    private ISnackbar Snackbar { get; set; }
    
    private bool _loading = true;
    private List<NoteDto> _notes = [];
    private HashSet<NoteDto> _selectedNotes = [];
    
    
    protected override async Task OnInitializedAsync()
    {
        var response = await NoteService.GetNotes();

        if (response.IsSuccess)
        {
            _notes = response.Value!;
            _loading = false;
        }
    }

    private async Task Download()
    {
        var ids = _selectedNotes.Select(n => n.Id).ToList();

        if (ids.Count == 0)
        {
            return;
        }
        
        var isSuccess = await ReportService.DownloadNoteReport(ids);

        if (!isSuccess)
        {
            Snackbar.Add(Translations.SomethingWentWrong, Severity.Error);
        }
    }
}