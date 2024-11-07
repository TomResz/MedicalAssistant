using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Infrastructure.DAL.Repository;
internal sealed class VisitNotificationRepository
	: IVisitNotificationRepository
{
	private readonly MedicalAssistantDbContext _context;

	public VisitNotificationRepository(MedicalAssistantDbContext context)
	{
		_context = context;
	}

	public void Add(VisitNotification visitNotification) 
		=> _context
			.VisitNotifications
			.Add(visitNotification);

	public void Delete(VisitNotification visitNotification) 
		=> _context
			.VisitNotifications
			.Remove(visitNotification);

	public void Update(VisitNotification visitNotification)
	{
		_context.VisitNotifications.Update(visitNotification);
	}
}
