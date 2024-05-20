using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Domain.Policies;
internal sealed class UserVisitPolicy : IVisitPolicy
{
	public bool UserMatchWithVisit(Visit visit, UserId userId)
		=> visit.UserId != userId;
}
