
using MedicalAssistant.Application.Abstraction;

namespace MedicalAssistant.Application.VisitNotifications.Commands.ChangeDate;
public record ChangeVisitNotificationDateCommand(
	Guid Id,
	DateTime Date,
	DateTime DateUtc,
	DateTime CurrentDate) : ICommand;
