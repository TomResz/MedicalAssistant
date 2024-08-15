
using MedicalAssist.Infrastructure.Notifications;
using Microsoft.AspNetCore.SignalR;

namespace MedicalAssist.API.Endpoints;

public class NotificationEndpoints : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
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
