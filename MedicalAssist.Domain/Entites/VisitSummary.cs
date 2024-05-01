using MedicalAssist.Domain.ValueObjects;
using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Domain.Entites;
public class VisitSummary
{
    public VisitSummaryId Id { get; private set; }
    public VisitId VisitId { get; private set; }

    public Date AddedDateUtc { get; private set; }

    private readonly HashSet<Recommendation> _recommendations = new();
    public IReadOnlySet<Recommendation> Recommendations => _recommendations;

    public VisitSummary(VisitSummaryId id,VisitId visitId,Date addedDateUtc)
    {
        Id = id;
        VisitId = visitId;
        AddedDateUtc = addedDateUtc;
    }
    private VisitSummary() { }

    public void AddRecommendation(Recommendation recommendation)
    {
        _recommendations.Add(recommendation);
    }
    public void RemoveRecommendation(Recommendation recommendation)
    {
        bool wasRemoved = _recommendations.Remove(recommendation);
        if(!wasRemoved)
        {
            //  TO DO
            // throw custom exception
        }
    }
}
