using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicalNotes.Queries;

public record GetMedicalNotesByTermAndTagsQuery(
    string[]? Tags,string? SearchTerm) : IQuery<IEnumerable<MedicalNoteDto>>;