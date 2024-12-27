using MedicalAssistant.Application.Abstraction;

namespace MedicalAssistant.Application.MedicalHistory.Commands.EditStage;

public record EditDiseaseStageCommand(
    Guid Id,
    Guid MedicalHistoryId,
    Guid? VisitId,
    string Name,
    string? Note,
    DateTime Date) : ICommand;