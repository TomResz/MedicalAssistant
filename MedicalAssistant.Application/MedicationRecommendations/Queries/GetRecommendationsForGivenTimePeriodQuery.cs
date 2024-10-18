using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicationRecommendations.Queries;
public sealed record GetRecommendationsForGivenTimePeriodQuery(
    Guid VisitId,
    DateTime Begin,
    DateTime End) : IRequest<IEnumerable<RecommendationDto>>;
