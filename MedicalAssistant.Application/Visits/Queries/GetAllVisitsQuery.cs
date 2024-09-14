using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Visits.Queries;
public sealed record GetAllVisitsQuery() : IRequest<IEnumerable<VisitDto>>;
