using MediatR;

namespace MedicalAssistant.Application.MedicalNotes.Commands.Add;

public sealed record AddMedicalNoteCommand(
    string Note,
    DateTime CreatedAt,
    string[] Tags) : IRequest<Guid>;