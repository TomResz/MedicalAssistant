using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicalNotes.Queries;

public sealed record GetMedicalNotesQuery() 
    : IRequest<IEnumerable<MedicalNoteDto>>;