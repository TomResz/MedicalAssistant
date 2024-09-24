using MediatR;

namespace MedicalAssistant.Application.VisitNotifications.Commands.ChangeDate;
public record ChangeVisitNotificationDateCommand(
	Guid Id,
	DateTime Date,
	DateTime DateUtc,
	DateTime CurrentDate) : IRequest;
