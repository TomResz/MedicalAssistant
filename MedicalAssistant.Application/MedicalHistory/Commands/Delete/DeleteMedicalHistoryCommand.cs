using MedicalAssistant.Application.Abstraction;

namespace MedicalAssistant.Application.MedicalHistory.Commands.Delete;

public sealed record DeleteMedicalHistoryCommand(Guid Id) 
    : ICommand;