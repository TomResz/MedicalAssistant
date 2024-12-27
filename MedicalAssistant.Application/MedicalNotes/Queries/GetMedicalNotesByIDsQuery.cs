using MediatR;
using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicalNotes.Queries;

public sealed record GetMedicalNotesByIDsQuery(List<Guid> Ids) 
    : IQuery<List<MedicalNoteDto>>;