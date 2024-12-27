using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicationRecommendations.Queries;
public sealed record GetRecommendationsForGivenTimePeriodQuery(
    Guid VisitId,
    DateTime Begin,
    DateTime End) : IQuery<IEnumerable<RecommendationDto>>;
