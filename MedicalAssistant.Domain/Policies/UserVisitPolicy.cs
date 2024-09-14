using MedicalAssistant.Domain.Entites;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Policies;
internal sealed class UserVisitPolicy : IVisitPolicy
{
	public bool UserMatchWithVisit(Visit visit, UserId userId)
		=> visit.UserId != userId;
}
