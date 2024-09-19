using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Domain.ValueObjects.IDs;
using Microsoft.AspNetCore.SignalR;

namespace MedicalAssistant.Infrastructure.Notifications;
internal sealed class NotificationSender : INotificationSender
{
	private readonly IHubContext<NotificationHub,INotificationsClient> _hubContext;

	public NotificationSender(IHubContext<NotificationHub, INotificationsClient> hubContext)
	{
		_hubContext = hubContext;
	}

	public async Task SendNotification(UserId userId,string content)
	{
		await _hubContext.Clients.User(userId.Value.ToString()).ReceiveNotification(content);
	}
}
