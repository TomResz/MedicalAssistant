using MediatR;
using MedicalAssistant.Application.Abstraction;

namespace MedicalAssistant.Application.MedicalNotes.Commands.Add;

public sealed record AddMedicalNoteCommand(
    string Note,
    DateTime CreatedAt,
    string[] Tags) : ICommand<Guid>;