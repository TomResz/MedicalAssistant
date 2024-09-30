using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Infrastructure.Notifications;

public interface INotificationsClient
{
	Task ReceiveNotification(NotificationDto notification);
}