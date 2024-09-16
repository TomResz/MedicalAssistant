using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.Visits;
using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace MedicalAssistant.Application.VisitNotifications.Events.SendNotification;

internal sealed class SendVisitNotificationEventHandler : INotificationHandler<SendVisitNotificationEvent>
{
	private readonly ILogger<SendVisitNotificationEventHandler> _logger;
	private readonly IClock _clock;
	private readonly IUserRepository _userRepository;
	private readonly IVisitRepository _visitRepository;
	private readonly IEmailService _emailService;

	public SendVisitNotificationEventHandler(
		ILogger<SendVisitNotificationEventHandler> logger,
		IClock clock,
		IUserRepository userRepository,
		IVisitRepository visitRepository,
		IEmailService emailService)
	{
		_logger = logger;
		_clock = clock;
		_userRepository = userRepository;
		_visitRepository = visitRepository;
		_emailService = emailService;
	}

	public async Task Handle(SendVisitNotificationEvent notification, CancellationToken cancellationToken)
	{
		var visit = await _visitRepository.GetByIdAsync(notification.VisitId,cancellationToken);

        if (visit is null)
        {
			throw new UnknownVisitException();
        }

		Domain.Entites.User? user = await _userRepository.GetByIdWithSettingsAsync(visit.UserId, cancellationToken);

        if (user is null)
        {
			throw new UserNotFoundException();
        }

		var language =  user.UserSettings.NotificationLanguage;

		if(user.UserSettings.EmailNotificationEnabled)
		{
			await _emailService.SendMailWithVisitNotification(user.Email,visit.ToDto(),language);
		}

        _logger.LogInformation($"Visit Notification published at: {_clock.GetCurrentUtc()}");
	}
}
