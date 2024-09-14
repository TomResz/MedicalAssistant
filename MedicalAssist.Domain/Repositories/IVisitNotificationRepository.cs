using MedicalAssist.Domain.Entites;

namespace MedicalAssist.Domain.Repositories;
public interface IVisitNotificationRepository
{
	void Add(VisitNotification visitNotification);
	void Delete(VisitNotification visitNotification);
}
