using MedicalAssistant.Domain.ComplexTypes;
using MedicalAssistant.Domain.Events;
using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.Primitives;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Entites;
public class Visit : AggregateRoot<VisitId>
{
    public UserId UserId { get; private set; }
    public Address Address { get; private set; }
    public Date Date { get; private set; }
    public Date PredictedEndDate { get; private set; } 

    public DoctorName DoctorName { get; private set; }
    public VisitDescription VisitDescription { get; private set; }
    public VisitType VisitType { get; private set; }

	private readonly HashSet<MedicationRecommendation> _recommendations = new();
    public IEnumerable<MedicationRecommendation> Recommendations => _recommendations;

    private readonly HashSet<VisitNotification> _notifications = new();

	public IEnumerable<VisitNotification> Notifications => _notifications;


    private readonly HashSet<Attachment> _attachments = new();
    public IEnumerable<Attachment> Attachments => _attachments;
    private Visit() { }

    private Visit(
		VisitId id,
		UserId userId,
		Address address,
		Date date,
		DoctorName doctorName,
		VisitDescription visitDescription,
		VisitType visitType,
		Date predictedEndDate)
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

    public static Visit Create(
		UserId userId,
		Address address,
		Date date,
		DoctorName doctorName,
		VisitDescription visitDescription,
		VisitType visitType,
		Date predictedEndDate)
    {
        if(date >= predictedEndDate)
        {
            throw new InvalidPredictedDateException();
        }

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
   
    public void AddNotification(VisitNotification notification)
    {
        _notifications.Add(notification);
    }

    public void AddRecommendation(MedicationRecommendation recommendation)
    {
        _recommendations.Add(recommendation);
        AddEvent(new RecommendationAddedEvent(Id,recommendation.Id));
    }

	public void DeleteRecommendation(MedicationRecommendationId recommendationId)
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

	public void Update(
		Address address,
		Date date,
		DoctorName doctorName,
		VisitDescription description,
		VisitType visitType,
		Date endDate)
	{
        Address = address;
        Date = date;
        DoctorName  = doctorName;
        VisitDescription = description;
        VisitType = visitType;
        PredictedEndDate = endDate; 
	}

	public void DeleteNotification(VisitNotification notification)
	{
        _notifications.Remove(notification);
    }

	public VisitNotification ChangeNotificationDate(Date dateUtc, VisitNotificationId notificationId)
	{
        var notification = _notifications.First(x=> x.Id == notificationId);
        var date = new Date(dateUtc);

        if(date >= Date)
        {
            throw new InvalidVisitNotificationDateException();
        }

        notification.ChangeDate(date);

        return notification;
    }
    
    public void AddAttachment(Attachment attachment)
    {
        _attachments.Add(attachment);
    }

	public void UpdateMedicationRecommendation(MedicationRecommendation recommendation)
	{
		throw new NotImplementedException();
	}
}
