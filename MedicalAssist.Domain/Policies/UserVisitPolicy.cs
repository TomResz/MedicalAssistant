using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Domain.Policies;
internal sealed class UserVisitPolicy : IVisitPolicy
{
	public bool CanAddRecommendation(Visit visit, UserId userId,out string reason)
	{
		if (!visit.WasVisited)
		{
			reason = "the visit has not been completed.";
			return false;
		}	
		if(visit.UserId !=  userId)
		{
			reason = "Another user cannot add a recommendation to a visit that is not theirs.";
			return false;
		}
		reason = "";
		return true;
	}
}
