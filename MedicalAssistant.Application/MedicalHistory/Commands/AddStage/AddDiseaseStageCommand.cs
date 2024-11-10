using MediatR;

namespace MedicalAssistant.Application.MedicalHistory.Commands.AddStage;

public sealed record AddDiseaseStageCommand(
    Guid MedicalHistoryId,
    Guid? VisitId,
    string? Note,
    string Name,
    DateTime Date) : IRequest<Guid>;
    