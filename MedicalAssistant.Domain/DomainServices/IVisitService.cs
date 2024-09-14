using MedicalAssistant.Domain.Entites;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.DomainServices;
public interface IVisitService
{
	void AddRecommendation(Visit visit, UserId userId, Recommendation recommendation);
	void RemoveRecommendation(Visit visit, UserId user, RecommendationId recommendation);
}
