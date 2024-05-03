using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Domain.Policies;
public interface IVisitPolicy
{
	bool CanAddRecommendation(Visit visit, UserId userId,out string reason);
}
