using MediatR;
using MedicalAssist.Application.Dto;

namespace MedicalAssist.Application.Recommendations.Queries;
public sealed record GetRecommendationsForGivenTimePeriodQuery(
    Guid VisitId,
    DateTime Begin,
    DateTime End) : IRequest<IEnumerable<RecommendationDto>>;
