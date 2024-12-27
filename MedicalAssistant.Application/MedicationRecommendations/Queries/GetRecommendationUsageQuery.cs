using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicationRecommendations.Queries;
public sealed record GetRecommendationUsageQuery(
	DateTime Date) : IQuery<IEnumerable<RecommendationUsageDto>>;