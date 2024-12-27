using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Visits.Queries;
public sealed record GetAllVisitsQuery(
	string Direction) : IQuery<IEnumerable<VisitDto>>;
