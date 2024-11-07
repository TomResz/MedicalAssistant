using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Policies;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.DomainServices;
internal class VisitService : IVisitService
{
	private readonly IVisitPolicy _policy;

	public VisitService(IVisitPolicy policy)
	{
		_policy = policy;
	}

	public void AddRecommendation(Visit visit, UserId userId, MedicationRecommendation recommendation)
	{
        ValidatePolicy(visit, userId);
		visit.AddRecommendation(recommendation);
	}

    public void RemoveRecommendation(Visit visit, UserId userId, MedicationRecommendationId recommendationId)
    {
        ValidatePolicy(visit, userId);
        visit.DeleteRecommendation(recommendationId);
    }

    private void ValidatePolicy(Visit visit, UserId userId)
    {
        if (!_policy.UserMatchWithVisit(visit, userId))
        {
            throw new UserIdDoesntMatchException();
        }
    }
}
