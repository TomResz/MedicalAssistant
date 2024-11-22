using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicalNotes.Queries;

public record GetMedicalNotesByTermAndTagsQuery(
    string[]? Tags,string? SearchTerm) : IRequest<IEnumerable<MedicalNoteDto>>;