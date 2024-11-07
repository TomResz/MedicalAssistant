using MediatR;

namespace MedicalAssistant.Application.MedicalHistory.Commands.Add;

public sealed record AddMedicalHistoryCommand(
    Guid? VisitId,
    DateTime StartDate,
    string Name,
    string? Notes,
    string? SymptomDescription) : IRequest<Guid>;