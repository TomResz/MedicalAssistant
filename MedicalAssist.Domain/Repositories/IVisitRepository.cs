using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.ValueObjects;
using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Domain.Repositories;
public interface IVisitRepository
{
	void Add(Visit visit);
	Task<Visit?> GetByIdWithRecommendationsAsync(VisitId visitId, CancellationToken cancellationToken);
	Task<Visit?> GetByIdAsync(VisitId visitId, CancellationToken cancellationToken);
	void Update(Visit visit);
	Task<bool> HasConflictingVisits(Guid id, Guid userId,Date date, Date endDate, CancellationToken cancellationToken);
	void Remove(Visit visit);
}
