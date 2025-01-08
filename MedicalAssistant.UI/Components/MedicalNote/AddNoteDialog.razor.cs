using MedicalAssistant.UI.Models.MedicalNotes;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Severity = MudBlazor.Severity;

namespace MedicalAssistant.UI.Components.MedicalNote;

public partial class AddNoteDialog
{
    [CascadingParameter] public MudDialogInstance DialogInstance { get; set; }
    [Inject] ILocalTimeProvider TimeProvider { get; set; }
    [Inject] IMedicalNoteService NoteService { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    
    private MudForm form;
    private readonly NoteModel _model = new();
    private readonly NoteModelValidator _validator = new();

    private void AddTag()
    {
        const int maxLength = 15;
        var currentTag = _model.CurrentTag.ToUpper();

        if (!string.IsNullOrWhiteSpace(currentTag) &&
            !_model.Tags.Contains(currentTag) &&
            currentTag.Length <= maxLength)
        {
            _model.Tags.Add(currentTag);
        }
        _model.CurrentTag = string.Empty;
        StateHasChanged();
    }

    private async Task Submit()
    {
        await form.Validate();
        if (!form.IsValid) return;

        var request = new AddNoteRequest()
        {
            Note = _model.Note,
            CreatedAt = await TimeProvider.CurrentDate(),
            Tags = _model.Tags.ToArray()
        };

        var response = await NoteService.Add(request);

        if (response.IsSuccess)
        {
            var noteDto = new NoteDto()
            {
                Id = response.Value!,
                Note = _model.Note,
                Tags = _model.Tags.ToArray(),
                CreatedAt = request.CreatedAt
            };
            DialogInstance.Close(noteDto);
            Snackbar.Add(Translations.NoteAdded, Severity.Success);
            return;
        }
        Snackbar.Add(Translations.SomethingWentWrong, Severity.Error);

    }
    private void Cancel() => DialogInstance.Cancel();

    private void DeleteTag(MudChip<string> chip)
    {
        _model.Tags.Remove(chip.Text!);
        StateHasChanged();
    }

}