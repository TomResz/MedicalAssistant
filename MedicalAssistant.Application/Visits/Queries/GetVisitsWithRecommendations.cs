using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Visits.Queries;

public record GetVisitsWithRecommendationsQuery()
    : IRequest<IEnumerable<VisitWithRecommendationsDto>>;