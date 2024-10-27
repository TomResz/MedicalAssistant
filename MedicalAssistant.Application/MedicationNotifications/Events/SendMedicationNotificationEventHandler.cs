using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Dto.Mappers;
using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.Entites;
using MedicalAssistant.Domain.Events;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects.IDs;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MedicalAssistant.Application.MedicationNotifications.Events;
internal sealed class SendMedicationNotificationEventHandler
	: INotificationHandler<SendMedicationNotificationEvent>
{
	private readonly ILogger<SendMedicationNotificationEventHandler> _logger;
	private readonly IMedicationRecommendationRepository _repository;
	private readonly IUserRepository _userRepository;
	private readonly INotificationSender _notificationSender;
	private readonly IClock _clock;
	private readonly INotificationHistoryRepository _historyRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IEmailService _emailService;

	public SendMedicationNotificationEventHandler(
		ILogger<SendMedicationNotificationEventHandler> logger,
		IMedicationRecommendationRepository repository,
		IUserRepository userRepository,
		INotificationSender notificationSender,
		IClock clock,
		INotificationHistoryRepository historyRepository,
		IUnitOfWork unitOfWork,
		IEmailService emailService)
	{
		_logger = logger;
		_repository = repository;
		_userRepository = userRepository;
		_notificationSender = notificationSender;
		_clock = clock;
		_historyRepository = historyRepository;
		_unitOfWork = unitOfWork;
		_emailService = emailService;
	}

	public async Task Handle(SendMedicationNotificationEvent @event, CancellationToken cancellationToken)
	{
		var recommendation = await _repository.GetWithNotificationsAsync(@event.RecommendationId, cancellationToken);

		if (recommendation is null)
		{
			_logger.LogWarning($"Recommendation with Id={@event.RecommendationId} does not exists.");
			return;
		}

		var notification = recommendation
			.Notifications
			.FirstOrDefault(x => x.Id == new MedicationRecommendationNotificationId(@event.NotificationId));

		if (notification is null)
		{
			_logger.LogWarning($"Medication recommendation with Id={@event.NotificationId} does not exists.");
			return;
		}

		var user = await _userRepository.GetByIdWithSettingsAsync(recommendation.UserId, cancellationToken);

		if (user is null)
		{
			return;
		}

		var settings = user.UserSettings;

		if (!settings.NotificationsEnabled)
		{
			return;
		}

		var dto = recommendation.ToDto();

		var contentJson = JsonSerializer.Serialize(dto);

		var notificationHistory = NotificationHistory.Create(
			user.Id,
			contentJson,
			"Medication_Recommendation",
			_clock.GetCurrentUtc());

		_historyRepository.Add(notificationHistory);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		var notificationDto = new NotificationDto
		{
			Id = notificationHistory.Id.Value,
			ContentJson = contentJson,
			Type = notificationHistory.Type.Value,
			WasRead = false,
		};


		await _notificationSender.SendNotification(user.Id, notificationDto);

		if (settings.EmailNotificationEnabled) 
		{
			await _emailService.SendMailWithMedicationRecommendation(user.Email, dto, settings.NotificationLanguage);
		}
		_logger.LogInformation($"Medication Notification published at: {_clock.GetCurrentUtc()}");

	}
}
