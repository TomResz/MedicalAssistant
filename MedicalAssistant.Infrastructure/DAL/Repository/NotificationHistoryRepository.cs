using MedicalAssistant.Domain.Entites;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

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

	public async Task MarkAsReadRange(List<Guid> ids,Date dateUtc)
	{
		await _context
			.NotificationHistories
			.Where(x => ids.Contains(x.Id))
			.ExecuteUpdateAsync(
			s => s
				.SetProperty(p => p.WasRead, true)
				.SetProperty(p => p.DateOfRead, dateUtc));
	}
}
