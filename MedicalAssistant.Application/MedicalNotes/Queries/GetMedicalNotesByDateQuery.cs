using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicalNotes.Queries;

public sealed record GetMedicalNotesByDateQuery(DateTime Date) 
    : IQuery<IEnumerable<MedicalNoteDto>>;