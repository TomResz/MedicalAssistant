using MediatR;
using MedicalAssist.Application.Dto;

namespace MedicalAssist.Application.Visits.Queries;
public sealed record GetCountOfVisitsByMonthQuery(
    int Month) : IRequest<IEnumerable<VisitDateAvailableCountDto>>;
