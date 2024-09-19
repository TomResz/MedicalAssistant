namespace MedicalAssistant.Infrastructure.Notifications;

public interface INotificationsClient
{
	Task ReceiveNotification(string content);
}