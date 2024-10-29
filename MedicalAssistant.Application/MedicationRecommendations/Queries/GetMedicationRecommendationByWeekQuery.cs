using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicationRecommendations.Queries;
public sealed record GetMedicationRecommendationByWeekQuery(
	DateTime Date) : IRequest<IEnumerable<MedicationRecommendationWithDayDto>>;
