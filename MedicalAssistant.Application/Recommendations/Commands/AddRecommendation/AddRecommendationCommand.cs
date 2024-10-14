using MediatR;

namespace MedicalAssistant.Application.Recommendations.Commands.AddRecommendation;
public sealed record AddRecommendationCommand(
    Guid VisitId,
    string? ExtraNote,
    string MedicineName,
    int Quantity,
    string[] TimeOfDay,
    DateTime StartDate,
    DateTime EndDate) : IRequest;
