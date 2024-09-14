using MediatR;

namespace MedicalAssistant.Application.Visits.Commands.DeleteVisit;
public sealed record DeleteVisitCommand(
	Guid VisitId) : IRequest;