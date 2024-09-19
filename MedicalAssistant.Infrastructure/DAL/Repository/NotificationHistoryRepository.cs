using MedicalAssistant.Domain.Entites;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Infrastructure.DAL.Repository;
internal sealed class NotificationHistoryRepository : INotificationHistoryRepository
{
	private readonly MedicalAssistDbContext _context;

	public NotificationHistoryRepository(MedicalAssistDbContext context) 
		=> _context = context;

	public void Add(NotificationHistory notificationHistory)
	{
		_context.NotificationHistories.Add(notificationHistory);
	}
}
