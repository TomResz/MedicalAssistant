using MedicalAssist.Domain.ComplexTypes;
using MedicalAssist.Domain.Events;
using MedicalAssist.Domain.Exceptions;
using MedicalAssist.Domain.Primitives;
using MedicalAssist.Domain.ValueObjects;
using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Domain.Entites;
public class Visit : AggregateRoot<VisitId>
{
    public UserId UserId { get; private set; }
    public Address Address { get; private set; }
    public Date Date { get; private set; }
    public DoctorName DoctorName { get; private set; }
    public VisitDescription VisitDescription { get; private set; }
    public VisitType VisitType { get; private set; }
    public bool WasVisited { get; private set; } = false;

	private readonly HashSet<Recommendation> _recommendations = new();
    public IEnumerable<Recommendation> Recommendations => _recommendations;
    private Visit() { }

    private Visit(VisitId id,UserId userId,Address address,Date date, DoctorName doctorName,VisitDescription visitDescription,VisitType visitType)
    {
        Id = id;
        UserId = userId;
        Address = address;
        Date = date;
        DoctorName = doctorName;
        VisitDescription = visitDescription;    
        VisitType = visitType;
    }

    public static Visit Create(UserId userId, Address address, Date date, DoctorName doctorName, VisitDescription visitDescription, VisitType visitType)
    {
        Visit visit = new Visit(Guid.NewGuid(),
			userId,
            address,
            date,
            doctorName,
            visitDescription,
            visitType);

        visit.AddEvent(new VisitCreatedEvent(userId, visit.Id));

        return visit;
    }

    internal void AddRecommendation(Recommendation recommendation)
    {
        _recommendations.Add(recommendation);
        AddEvent(new RecommendationAddedEvent(Id,recommendation.Id));
    }

	public void DeleteRecommendation(RecommendationId recommendationId)
	{
        bool wasRemoved = _recommendations.RemoveWhere(x=>x.Id == recommendationId) > 0;
        if (!wasRemoved)
        {
            throw new UnknownRecommendationException(recommendationId);
        }
        AddEvent(new RecommendationDeletedEvent(Id, recommendationId));
	}

	public void ChangeDate(Date date)
    {
        if(Date == date)
        {
            throw new SameDateException();
        }
        Date = date;
        AddEvent(new VisitDateChangedEvent(Id,date));
    }

}
