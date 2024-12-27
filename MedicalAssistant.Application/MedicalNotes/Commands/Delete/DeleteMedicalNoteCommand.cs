using MedicalAssistant.Application.Abstraction;

namespace MedicalAssistant.Application.MedicalNotes.Commands.Delete;

public sealed record DeleteMedicalNoteCommand(Guid Id) : ICommand;
