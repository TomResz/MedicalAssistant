using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicationRecommendations.Queries;
public sealed record GetRecommendationsForVisitQuery(
    Guid VisitId) : IRequest<IEnumerable<RecommendationDto>>;
