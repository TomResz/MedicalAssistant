using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Domain.Repositories;
public interface IVisitRepository
{
	void Add(Visit visit);
	Task<Visit?> GetByIdAsync(VisitId visitId, CancellationToken cancellationToken);
	void Update(Visit visit);
}
