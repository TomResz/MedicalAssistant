using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Domain.DomainServices;
public interface IVisitService
{
	void AddRecommendation(Visit visit, UserId userId, Recommendation recommendation);
	void RemoveRecommendation(Visit visit, UserId user, RecommendationId recommendation);
}
