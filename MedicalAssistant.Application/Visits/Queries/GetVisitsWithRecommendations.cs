using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Visits.Queries;

public record GetVisitsWithRecommendationsQuery(
    List<Guid> Ids): IQuery<IEnumerable<VisitWithRecommendationsDto>>;