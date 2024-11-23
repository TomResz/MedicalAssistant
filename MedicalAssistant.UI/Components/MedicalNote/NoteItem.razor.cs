using MedicalAssistant.UI.Models.MedicalNotes;
using MedicalAssistant.UI.Shared.Resources;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.MedicalNote;

public partial class NoteItem
{
    [Parameter] public string? Class { get; set; }
    [Parameter] public required NoteDto Note { get; set; }
    [Parameter] public EventCallback<Guid> OnDeleted { get; set; }
    [Parameter] public EventCallback<NoteDto> OnEdited { get; set; }
    [Inject] IDialogService DialogService { get; set; }
    private async Task Edit()
    {
        var parameters = new DialogParameters()
        {
            { nameof(EditNoteDialog.Note), Note }
        };

        var options = new DialogOptions()
        {
            MaxWidth = MaxWidth.Medium,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<EditNoteDialog>(
            Translations.Edit,
            parameters,
            options);
        
        var result = await dialog.Result;
        if (result is not null &&
            !result.Canceled &&
            result.Data is NoteDto dto)
        {
            
            await OnEdited.InvokeAsync(dto);
        }
    }

    private async Task Delete()
    {
        bool? result = await DialogService.ShowMessageBox(
            Translations.DialogDeleteNoteTitle,
            (MarkupString)Translations.DialogDeleteNote,
            yesText: Translations.DialogYes, cancelText: Translations.DialogNo);

        if (result is null or false)
        {
            return;
        }

        await OnDeleted.InvokeAsync(Note.Id);
    }
}