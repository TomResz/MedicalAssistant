using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicationRecommendations.Queries;

public sealed record GetMedicationRecommendationByDateQuery(
    DateTime Date) : IQuery<IEnumerable<MedicationRecommendationDto>>;
