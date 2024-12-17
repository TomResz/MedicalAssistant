using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicationRecommendations.Queries;
public sealed record GetRecommendationUsageQuery(
	DateTime Date) : IRequest<IEnumerable<RecommendationUsageDto>>;