using MediatR;

namespace MedicalAssistant.Application.MedicalNotes.Commands.Edit;

public sealed record EditMedicalNoteCommand(
    Guid Id,
    string Note,
    string[] Tags) : IRequest;