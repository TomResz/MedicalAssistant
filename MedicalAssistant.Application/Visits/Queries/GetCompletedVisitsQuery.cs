using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Visits.Queries;
public sealed record GetCompletedVisitsQuery(
	DateTime CurrentDate) : IRequest<IEnumerable<VisitDto>>;
