using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicalNotes.Queries;

public sealed record GetMedicalNotesByDateQuery(DateTime Date) 
    : IRequest<IEnumerable<MedicalNoteDto>>;