using MediatR;

namespace MedicalAssist.Application.Visits.Commands.ConfirmVisit;
public sealed record ConfirmVisitCommand(Guid VisistId) : IRequest;
