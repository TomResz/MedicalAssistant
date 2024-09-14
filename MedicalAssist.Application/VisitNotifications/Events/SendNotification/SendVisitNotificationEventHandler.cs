using MediatR;
using MedicalAssist.Domain.Abstraction;
using Microsoft.Extensions.Logging;

namespace MedicalAssist.Application.VisitNotifications.Events.SendNotification;

internal sealed class SendVisitNotificationEventHandler : INotificationHandler<SendVisitNotificationEvent>
{
	private readonly ILogger<SendVisitNotificationEventHandler> _logger;
	private readonly IClock _clock;

	public SendVisitNotificationEventHandler(
		ILogger<SendVisitNotificationEventHandler> logger, 
		IClock clock)
	{
		_logger = logger;
		_clock = clock;
	}

	public Task Handle(SendVisitNotificationEvent notification, CancellationToken cancellationToken)
	{
		_logger.LogInformation($"Visit Notification published at: {_clock.GetCurrentUtc()}");
		return Task.CompletedTask;
	}
}
