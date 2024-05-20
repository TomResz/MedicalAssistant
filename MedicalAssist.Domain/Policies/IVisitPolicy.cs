using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Domain.Policies;
public interface IVisitPolicy
{
	bool UserMatchWithVisit(Visit visit, UserId userId);
}
