using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.Repositories;

namespace MedicalAssist.Infrastructure.DAL.Repository;
internal sealed class VisitNotificationRepository
	: IVisitNotificationRepository
{
	private readonly MedicalAssistDbContext _context;

	public VisitNotificationRepository(MedicalAssistDbContext context)
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
}
