using MediatR;
using MedicalAssist.Application.Dto;

namespace MedicalAssist.Application.Recommendations.Queries;
public sealed record GetRecommendationsForVisitQuery(
    Guid VisitId) : IRequest<IEnumerable<RecommendationDto>>;
