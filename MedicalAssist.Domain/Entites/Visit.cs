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
    public Date PredictedEndDate { get; private set; } 

    public DoctorName DoctorName { get; private set; }
    public VisitDescription VisitDescription { get; private set; }
    public VisitType VisitType { get; private set; }

	private readonly HashSet<Recommendation> _recommendations = new();
    public IEnumerable<Recommendation> Recommendations => _recommendations;
    private Visit() { }

    private Visit(VisitId id,UserId userId,Address address,Date date, DoctorName doctorName,VisitDescription visitDescription,VisitType visitType,Date predictedEndDate)
    {
        Id = id;
        UserId = userId;
        Address = address;
        Date = date;
        DoctorName = doctorName;
        VisitDescription = visitDescription;    
        VisitType = visitType;
        PredictedEndDate = predictedEndDate;
    }

    public static Visit Create(UserId userId, Address address, Date date, DoctorName doctorName, VisitDescription visitDescription, VisitType visitType,Date predictedEndDate)
    {
        Visit visit = new(Guid.NewGuid(),
			userId,
            address,
            date,
            doctorName,
            visitDescription,
            visitType,
            predictedEndDate);

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

	public void Update(Address address, Date date, DoctorName doctorName, VisitDescription description, VisitType visitType, Date endDate)
	{
        Address = address;
        Date = date;
        DoctorName  = doctorName;
        VisitDescription = description;
        VisitType = visitType;
        PredictedEndDate = endDate; 
	}
}
