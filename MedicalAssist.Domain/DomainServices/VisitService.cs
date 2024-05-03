using MedicalAssist.Domain.Entites;
using MedicalAssist.Domain.Exceptions;
using MedicalAssist.Domain.Policies;
using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Domain.DomainServices;
internal class VisitService : IVisitService
{
	private readonly IVisitPolicy _policy;

	public VisitService(IVisitPolicy policy)
	{
		_policy = policy;
	}

	public void AddRecommendation(Visit visit, UserId userId, Recommendation recommendation)
	{

		if(!_policy.CanAddRecommendation(visit,userId,out string reason)) 
		{
			throw new CannotAddRecommendationException(reason);
		}

		visit.AddRecommendation(recommendation);
	}
}
