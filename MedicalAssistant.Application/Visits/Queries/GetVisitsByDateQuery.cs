using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Visits.Queries;
public sealed record GetVisitsByDateQuery(
	DateTime Date) : IQuery<IEnumerable<VisitDto>>;