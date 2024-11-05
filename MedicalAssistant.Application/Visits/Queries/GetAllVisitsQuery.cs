using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Visits.Queries;
public sealed record GetAllVisitsQuery(
	string Direction) : IRequest<IEnumerable<VisitDto>>;
