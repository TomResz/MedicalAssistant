using MedicalAssistant.Domain.Entities;

namespace MedicalAssistant.Domain.Repositories;
public interface IVisitNotificationRepository
{
	void Add(VisitNotification visitNotification);
	void Delete(VisitNotification visitNotification);
	void Update(VisitNotification visitNotification);
}
