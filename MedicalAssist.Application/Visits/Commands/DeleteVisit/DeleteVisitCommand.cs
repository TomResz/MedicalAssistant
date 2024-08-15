using MediatR;

namespace MedicalAssist.Application.Visits.Commands.DeleteVisit;
public sealed record DeleteVisitCommand(
	Guid VisitId) : IRequest;