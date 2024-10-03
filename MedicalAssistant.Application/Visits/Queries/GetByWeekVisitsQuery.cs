using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Visits.Queries;

public sealed record GetByWeekVisitQuery(
	DateTime Date) : IRequest<IEnumerable<VisitDto>>;