using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicationRecommendations.Queries;
public sealed record GetAllMedicationRecommendationsQuery() 
	: IRequest<IEnumerable<MedicationRecommendationDto>>;
