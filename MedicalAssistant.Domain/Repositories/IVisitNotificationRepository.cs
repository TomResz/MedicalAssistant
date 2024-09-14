using MedicalAssistant.Domain.Entites;

namespace MedicalAssistant.Domain.Repositories;
public interface IVisitNotificationRepository
{
	void Add(VisitNotification visitNotification);
	void Delete(VisitNotification visitNotification);
}
