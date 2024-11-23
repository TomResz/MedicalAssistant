using MedicalAssistant.UI.Models.MedicalNotes;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MedicalAssistant.UI.Components.MedicalNote;

public partial class EditNoteDialog
{
    [Parameter] public NoteDto Note { get; set; }
    [CascadingParameter] public MudDialogInstance DialogInstance { get; set; }
    [Inject] ILocalTimeProvider TimeProvider { get; set; }
    [Inject] IMedicalNoteRepository NoteRepository { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }

    private MudForm form;
    private readonly NoteModel _model = new();
    private readonly NoteModelValidator _validator = new();

    protected override void OnParametersSet()
    {
        _model.Tags = Note.Tags.ToList();
        _model.Note = Note.Note;
    }

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

        var request = new EditNoteRequest()
        {
            Id = Note.Id,
            Note = _model.Note,
            Tags = _model.Tags.ToArray()
        };

        var response = await NoteRepository.Edit(request);

        if (response.IsSuccess)
        {
            var noteDto = new NoteDto()
            {
                Id = Note.Id,
                Note = _model.Note,
                Tags = _model.Tags.ToArray(),
                CreatedAt = Note.CreatedAt,
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