﻿using MedicalAssistant.UI.Components.MedicalNote;
using MedicalAssistant.UI.Models.MedicalNotes;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Pages.Dashboard.Notes;

public partial class MedicalNotesPage
{
    [Inject] IMedicalNoteService NoteService { get; set; }
    [Inject] IDialogService DialogService { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    public string SearchTerm { get; set; }
    private bool _loading = true;
    private List<TagDto> _tags = [];
    private IEnumerable<string> _selectedTags { get; set; } = new HashSet<string>();
    private List<NoteDto> _notes = [];

    protected override async Task OnInitializedAsync()
    {
        var response = await NoteService.GetTags();
        
        if (response.IsSuccess)
        {
            _tags = response.Value!.ToList();
        }

        var notesResponse = await NoteService.GetNotes();

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

        var response = await NoteService.GetBySearchTermAndTags(searchTerm, tags);
        
        if (response.IsSuccess)
        {
            _notes = response.Value!;
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task NoteEdited(NoteDto note)
    {
        await OnInitializedAsync();
        await InvokeAsync(StateHasChanged);
    }
    
    private async Task NoteDeleted(Guid id)
    {
        var response = await NoteService.Delete(id);

        if (response.IsFailure)
        {
            Snackbar.Add(Translations.SomethingWentWrong, Severity.Error);
            return;
        }

        await Task.Delay(250);
        await OnInitializedAsync();
        await InvokeAsync(StateHasChanged);
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