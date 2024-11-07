using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.Repositories;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace MedicalAssistant.Infrastructure.DAL.Repository;
internal sealed class VisitRepository : IVisitRepository
{
	private readonly MedicalAssistantDbContext _context;

	public VisitRepository(MedicalAssistantDbContext context) => _context = context;

	public void Add(Visit visit)
		=> _context.Visits.Add(visit);

	public async Task<bool> Exists(VisitId id, CancellationToken cancellationToken)
		=> await _context.Visits.AnyAsync(x=>x.Id == id,cancellationToken);

	public async Task<Visit?> GetByIdAsync(VisitId visitId, CancellationToken cancellationToken)
		=> await _context
		.Visits
		.FirstOrDefaultAsync(x => x.Id == visitId, cancellationToken);

	public async Task<Visit?> GetByIdWithNotificationsAsync(VisitId visitId, CancellationToken cancellationToken)
		=> await _context
		.Visits
		.Include(x=>x.Notifications)
		.FirstOrDefaultAsync(x=>x.Id == visitId,
				cancellationToken);

	public async Task<Visit?> GetByIdWithRecommendationsAsync(VisitId visitId, CancellationToken cancellationToken)
		=> await _context
		.Visits
		.Include(x => x.Recommendations)
		.FirstOrDefaultAsync(x => x.Id == visitId, cancellationToken);

	public async Task<Visit?> GetByNotificationId(VisitNotificationId visitNotificationId, CancellationToken cancellationToken) 
		=> await _context
			.Visits
			.Include(x => x.Notifications)
			.FirstOrDefaultAsync(x => x.Notifications.Any(
					n => n.Id == visitNotificationId),
					cancellationToken);

	public async Task<bool> HasConflictingVisits(Guid id, Guid userId, Date date, Date endDate, CancellationToken cancellationToken)
		=> await _context
		.Visits
		.AnyAsync(x => x.Id != new VisitId(id) &&
			x.UserId == new UserId(userId) &&
			date < x.PredictedEndDate &&
			endDate > x.Date,
			cancellationToken);

	public void Remove(Visit visit)
		=> _context.Visits.Remove(visit);

	public void Update(Visit visit)
		=> _context
		.Visits
		.Update(visit);
}
