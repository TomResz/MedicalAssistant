using MedicalAssistant.Domain.Exceptions.IDs;

namespace MedicalAssistant.Domain.ValueObjects.IDs;
public sealed record NotificationHistoryId
{
	public Guid Value { get; }

    public NotificationHistoryId(Guid value)
    {
        if(value == Guid.Empty)
        {
            throw new InvalidNotificationHistoryIdException();
		}
        Value = value;
    }
    public static implicit operator Guid(NotificationHistoryId value)  => value.Value;
    public static implicit operator NotificationHistoryId(Guid value) => new(value);
}
