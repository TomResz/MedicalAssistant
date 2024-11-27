using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Visits.Queries;

public record GetVisitsWithRecommendationsQuery(
    List<Guid> Ids): IRequest<IEnumerable<VisitWithRecommendationsDto>>;