using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Repositories;
public interface IVisitRepository
{
	void Add(Visit visit);
	Task<Visit?> GetByIdWithRecommendationsAsync(VisitId visitId, CancellationToken cancellationToken);
	Task<Visit?> GetByIdAsync(VisitId visitId, CancellationToken cancellationToken);
	void Update(Visit visit);
	Task<bool> HasConflictingVisits(Guid id, Guid userId,Date date, Date endDate, CancellationToken cancellationToken);
	void Remove(Visit visit);
	Task<Visit?> GetByIdWithNotificationsAsync(VisitId visitId,CancellationToken cancellationToken);
	Task<Visit?> GetByNotificationId(VisitNotificationId visitNotificationId, CancellationToken cancellationToken);

	Task<bool> Exists(VisitId id,CancellationToken cancellationToken);
}
