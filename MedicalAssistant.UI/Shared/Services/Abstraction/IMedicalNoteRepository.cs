using MedicalAssistant.UI.Components.MedicalNote;
using MedicalAssistant.UI.Models.MedicalNotes;

namespace MedicalAssistant.UI.Shared.Services.Abstraction;

public interface IMedicalNoteRepository
{
    Task<Response.Base.Response<IEnumerable<TagDto>>> GetTags();
    
    Task<Response.Base.Response<List<NoteDto>>> GetNotes();
    Task<Response.Base.Response<List<NoteDto>>> GetBySearchTermAndTags(string? searchTerm,string[]? tags);
    Task<Response.Base.Response<Guid>> Add(AddNoteRequest request);
    Task<Response.Base.Response> Edit(EditNoteRequest request);
    Task<Response.Base.Response> Delete(Guid id);
    Task<Response.Base.Response<List<NoteDto>>> GetCurrents(DateTime currentDate);
}