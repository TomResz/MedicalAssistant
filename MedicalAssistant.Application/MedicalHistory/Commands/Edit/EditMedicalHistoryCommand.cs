using MediatR;

namespace MedicalAssistant.Application.MedicalHistory.Commands.Edit;

public sealed record EditMedicalHistoryCommand(
    Guid Id,
    Guid? VisitId,
    DateTime StartDate,
    DateTime? EndDate,
    string Name,
    string? Notes,
    string? SymptomDescription) : IRequest;