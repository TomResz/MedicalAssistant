using MedicalAssistant.Infrastructure.Notifications;
using Microsoft.AspNetCore.SignalR;

namespace MedicalAssistant.API.Endpoints;

public class NotificationEndpoints : IEndpoints
{
	public void MapEndpoints(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("notifacation")
			.WithTags("Notifications");

		group.MapPost("/", async (
			IHubContext<NotificationHub, INotificationsClient> _context,
			string message,
			Guid userId) =>
		{
			await _context.Clients.User(userId.ToString()).ReceiveNotification(message);
		});
	}
}
