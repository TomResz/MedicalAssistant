using MedicalAssistant.UI.Components.MedicalNote;
using MedicalAssistant.UI.Models.MedicalNotes;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Pages.Dashboard.Notes;

public partial class MedicalNotesPage
{
    [Inject] IMedicalNoteRepository NoteRepository { get; set; }
    [Inject] IDialogService DialogService { get; set; }
    
    public string SearchTerm { get; set; }
    private bool _loading = true;
    private List<TagDto> _tags = [];
    private IEnumerable<string> _selectedTags { get; set; } = new HashSet<string>();
    private List<NoteDto> _notes = [];

    protected override async Task OnInitializedAsync()
    {
        var response = await NoteRepository.GetTags();
        
        if (response.IsSuccess)
        {
            _tags = response.Value!.ToList();
        }

        var notesResponse = await NoteRepository.GetNotes();

        if (response.IsSuccess)
        {
            _notes = notesResponse.Value!;
        }

        _loading = false;
    }
    private async Task Search()
    {
        await Task.CompletedTask;
    }

    private async Task Filter()
    {
        var searchTerm = SearchTerm;
        var tags = _selectedTags.ToArray();

        var response = await NoteRepository.GetBySearchTermAndTags(searchTerm, tags);
        
        if (response.IsSuccess)
        {
            _notes = response.Value!;
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task NoteDeleted(Guid id)
    {
        var note = _notes.FirstOrDefault(x => x.Id == id);
        if (note is not null)
        {
            _notes.Remove(note);
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task Add()
    {
        var options = new MudBlazor.DialogOptions()
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
        };
        var dialog = await DialogService.ShowAsync<AddNoteDialog>(Translations.Add,options);
        var result = await dialog.Result;

        if (result is not null &&
            !result.Canceled &&
            result.Data is NoteDto note)
        {
            await OnInitializedAsync();
            await InvokeAsync(StateHasChanged);
        }
    }

}