using MedicalAssistant.Domain.Entites;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Policies;
public interface IVisitPolicy
{
	bool UserMatchWithVisit(Visit visit, UserId userId);
}
