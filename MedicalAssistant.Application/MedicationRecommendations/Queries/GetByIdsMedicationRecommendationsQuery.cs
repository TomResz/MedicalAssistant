using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicationRecommendations.Queries;

public sealed record GetByIdsMedicationRecommendationsQuery(List<Guid> IDs) : IRequest<IEnumerable<MedicationRecommendationDto>>;