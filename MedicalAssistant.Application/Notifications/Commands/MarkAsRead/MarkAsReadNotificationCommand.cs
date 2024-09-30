using MediatR;

namespace MedicalAssistant.Application.Notifications.Commands.MarkAsRead;
public sealed record MarkAsReadNotificationCommand(
	List<Guid> IDs) : IRequest;

