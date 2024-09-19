using MedicalAssistant.Domain.Entites;

namespace MedicalAssistant.Domain.Repositories;
public interface INotificationHistoryRepository
{
	void Add(NotificationHistory notificationHistory);
}
