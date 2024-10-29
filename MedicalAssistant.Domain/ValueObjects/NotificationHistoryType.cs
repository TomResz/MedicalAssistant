using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.ValueObjects;
public sealed record NotificationHistoryType
{
    public const string VisitNotification = "VISIT_NOTIFICATION";
    public const string MedicationNotification = "MEDICATION_NOTIFICATION";

	public string Value { get; }

    public NotificationHistoryType(string value)
    {
        if(string.IsNullOrEmpty(value))
        {
            throw new EmptyNotificationHistoryTypeException();
		}
        Value = value;
    }
    public static implicit operator string(NotificationHistoryType value) => value.Value;   
    public static implicit operator NotificationHistoryType(string value) => new(value);
}
