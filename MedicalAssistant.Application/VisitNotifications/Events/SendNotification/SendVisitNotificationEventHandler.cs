using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Exceptions;
using MedicalAssistant.Application.Visits;
using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.Entites;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MedicalAssistant.Application.VisitNotifications.Events.SendNotification;

internal sealed class SendVisitNotificationEventHandler
	: INotificationHandler<SendVisitNotificationEvent>
{
	private readonly ILogger<SendVisitNotificationEventHandler> _logger;
	private readonly IUserRepository _userRepository;
	private readonly IVisitRepository _visitRepository;
	private readonly IClock _clock;
	private readonly IEmailService _emailService;
	private readonly INotificationSender _notificationSender;
	private readonly INotificationHistoryRepository _notificationHistoryRepository;
	private readonly IUnitOfWork _unitOfWork;

	public SendVisitNotificationEventHandler(
		ILogger<SendVisitNotificationEventHandler> logger,
		IClock clock,
		IUserRepository userRepository,
		IVisitRepository visitRepository,
		IEmailService emailService,
		INotificationSender notificationSender,
		INotificationHistoryRepository notificationHistoryRepository,
		IUnitOfWork unitOfWork)
	{
		_logger = logger;
		_clock = clock;
		_userRepository = userRepository;
		_visitRepository = visitRepository;
		_emailService = emailService;
		_notificationSender = notificationSender;
		_notificationHistoryRepository = notificationHistoryRepository;
		_unitOfWork = unitOfWork;
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
		var visitDto = visit.ToDto();

		string contentJson = JsonSerializer.Serialize(visitDto);
		var notificationObj = NotificationHistory.Create(
			user.Id,
			contentJson,
			"Visit Notification",
			  _clock.GetCurrentUtc());

		_notificationHistoryRepository.Add(notificationObj);
		
		var notificationDto = new NotificationDto
		{
			Id = notificationObj.Id.Value,
			ContentJson = contentJson,
			Type = notificationObj.Type.Value,
			WasRead = false,
		};

		await _unitOfWork.SaveChangesAsync(cancellationToken);	

		if (user.UserSettings.NotificationsEnabled)
		{
			await _notificationSender.SendNotification(user.Id, notificationDto);
		}

		if(user.UserSettings.EmailNotificationEnabled)
		{
			await _emailService.SendMailWithVisitNotification(user.Email,visitDto,language);
		}

        _logger.LogInformation($"Visit Notification published at: {_clock.GetCurrentUtc()}");
	}
}
