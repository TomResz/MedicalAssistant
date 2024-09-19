using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace MedicalAssistant.Infrastructure.Notifications;

[Authorize]
public class NotificationHub : Hub<INotificationsClient>
{
	private readonly ILogger<NotificationHub> _logger;

	public NotificationHub(ILogger<NotificationHub> logger)
	{
		_logger = logger;
	}

	public override Task OnConnectedAsync()
	{
		var userId = Context.UserIdentifier;
		_logger.LogInformation($"Client with {userId} connected to hub.");
		return Task.CompletedTask;
	}
}
