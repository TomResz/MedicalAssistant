using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Visits.Queries;
public sealed record GetCompletedVisitsQuery(
	DateTime CurrentDate) : IQuery<IEnumerable<VisitDto>>;
