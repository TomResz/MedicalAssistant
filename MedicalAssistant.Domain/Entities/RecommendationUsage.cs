using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Entities;

public class RecommendationUsage
{
	public RecommendationUsageId Id { get; set; }
	public string TimeOfDay { get; private set; }
	public Date Date { get; private set; }
	public MedicationRecommendationId MedicationRecommendationId { get; private set; }

#region EF CORE
	public MedicationRecommendation MedicationRecommendation { get; private set; }
	private RecommendationUsage() { }
#endregion

	public RecommendationUsage(
		RecommendationUsageId id,
		string timeOfDay,
		Date date,
		MedicationRecommendationId medicationRecommendationId)
    {
        Id = id;
		TimeOfDay = timeOfDay;
		Date = date;
		MedicationRecommendationId = medicationRecommendationId;
    }

	public static RecommendationUsage Create(
		string timeOfDay,
		Date createdAt,
		MedicationRecommendationId medicationRecommendationId)
	{
		Date date = new(createdAt.Value.Date);
		return new RecommendationUsage(
			Guid.NewGuid(),
			timeOfDay,
			date,
			medicationRecommendationId);
	}
}


public sealed record RecommendationUsageId
{
	public Guid Value { get; }

    public RecommendationUsageId(Guid value)
    {
        Value = value;
    }

	public static implicit operator Guid(RecommendationUsageId value) => value.Value;
	public static implicit operator RecommendationUsageId(Guid value) => new(value);
}