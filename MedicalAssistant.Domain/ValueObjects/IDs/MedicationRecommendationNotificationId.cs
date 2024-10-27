using MedicalAssistant.Domain.Exceptions.IDs;

namespace MedicalAssistant.Domain.ValueObjects.IDs;
public sealed record MedicationRecommendationNotificationId
{
	public Guid Value { get; }

    public MedicationRecommendationNotificationId(Guid value)
    {
        if(value == Guid.Empty)
        {
            throw new InvalidMedicationRecommendationNotificationIdException();

		}
        Value = value;
    }

    public static implicit operator Guid(MedicationRecommendationNotificationId value) => value.Value;
    public static implicit operator MedicationRecommendationNotificationId(Guid value) => new(value);
}
