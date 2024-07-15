using MediatR;
using MedicalAssist.Application.Dto;
using MedicalAssist.Application.Pagination;

namespace MedicalAssist.Application.Visits.Queries;
public sealed record GetPageOfCurrentVisitsQuery(
    int Page,
    int PageSize,
    OrderDirection Direction,
    int DaysAhead,
    int DaysBack) : IRequest<PagedList<VisitDto>>;
