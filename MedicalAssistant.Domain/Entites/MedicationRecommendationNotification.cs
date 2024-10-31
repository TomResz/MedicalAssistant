using MedicalAssistant.Domain.Exceptions;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Entites;
public class MedicationRecommendationNotification
{
	public MedicationRecommendationNotificationId Id { get; private set; }
	public MedicationRecommendationId MedicationRecommendationId { get; private set; }
	public string JobId { get; private set; }
	public Date Start { get; private set; }
	public Date End { get; private set; }
	public TimeOnly TriggerTimeUtc { get; private set; }

	private MedicationRecommendationNotification() { }

	private MedicationRecommendationNotification(
		MedicationRecommendationNotificationId id,
		MedicationRecommendationId medicationRecommendationId,
		Date start,
		Date end,
		TimeOnly triggerTimeUtc,
		string jobId)
	{
		Id = id;
		MedicationRecommendationId = medicationRecommendationId;
		Start = start;
		End = end;
		TriggerTimeUtc = triggerTimeUtc;
		JobId = jobId;
	}

	public static MedicationRecommendationNotification Create(
		MedicationRecommendationId medicationRecommendationId,
		Date start,
		Date end,
		TimeOnly triggerTimeUtc)
	{
		Date startDate = new(start.Value.Date);
		Date endDate = new(end.Value.Date.AddDays(1).AddTicks(-1));

		if(endDate < start)
		{
			throw new SameDateException();
		}

		var id = Guid.NewGuid();
		return new MedicationRecommendationNotification(
			id,
			medicationRecommendationId,
			startDate,
			endDate,
			triggerTimeUtc,
			$"medication-notification-{id}");
	}

	internal void ChangeTriggerTime(TimeOnly newTriggerTime)
	{
		TriggerTimeUtc = newTriggerTime;
	}


	public void ChangeJobId(string jobId)
	{
		JobId = jobId;
	}

	public void Edit(Date start, Date end, TimeOnly triggerTimeUtc)
	{
		if (end < start)
		{
			throw new SameDateException();
		}
		Start = start;
		End = end;
		TriggerTimeUtc = triggerTimeUtc;
	}
}
