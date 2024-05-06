using MediatR;

namespace MedicalAssist.Application.Recommendations.Commands.DeleteRecommendation;
public sealed record DeleteRecommendationCommand(
    Guid VisitId,
    Guid RecommendationId) : IRequest;
