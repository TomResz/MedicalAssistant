using MediatR;

namespace MedicalAssist.Application.Recommendations.Commands.AddRecommendation;
public sealed record AddRecommendationCommand(
    Guid VisitId,
    string? ExtraNote,
    string MedicineName,
    int Quantity,
    string TimeOfDay) : IRequest;
