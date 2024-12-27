using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Visits.Queries;

public sealed record GetVisitBySerchTermQuery(
    string SearchTerm,
    string Direction) : IQuery<IEnumerable<VisitDto>>;
