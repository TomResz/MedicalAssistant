using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicationRecommendations.Queries;
public sealed record GetAllMedicationRecommendationsQuery() 
	: IQuery<IEnumerable<MedicationRecommendationDto>>;
