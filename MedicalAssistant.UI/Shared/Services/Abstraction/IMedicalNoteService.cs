using MedicalAssistant.UI.Models.MedicalNotes;
using MedicalAssistant.UI.Shared.Response.Base;

namespace MedicalAssistant.UI.Shared.Services.Abstraction;

public interface IMedicalNoteService
{
    Task<Response<IEnumerable<TagDto>>> GetTags();
    
    Task<Response<List<NoteDto>>> GetNotes();
    Task<Response<List<NoteDto>>> GetBySearchTermAndTags(string? searchTerm,string[]? tags);
    Task<Response<Guid>> Add(AddNoteRequest request);
    Task<Response.Base.Response> Edit(EditNoteRequest request);
    Task<Response.Base.Response> Delete(Guid id);
    Task<Response<List<NoteDto>>> GetCurrents(DateTime currentDate);
}