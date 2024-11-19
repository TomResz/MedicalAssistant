using MediatR;

namespace MedicalAssistant.Application.MedicalHistory.Commands.DeleteStage;

public record DeleteDiseaseStageCommand(
    Guid Id,
    Guid MedicalHistoryId) : IRequest;