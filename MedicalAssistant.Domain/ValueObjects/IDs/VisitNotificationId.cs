using MedicalAssistant.Domain.Exceptions.IDs;

namespace MedicalAssistant.Domain.ValueObjects.IDs;
public sealed record VisitNotificationId
{
	public Guid Value { get; }

    public VisitNotificationId(Guid value)
    {
        if(value == Guid.Empty)
        {
            throw new InvalidVisitNotificationIdException();
		}
		Value = value;
    }

    public static implicit operator VisitNotificationId(Guid value) => new(value);
    public static implicit operator Guid(VisitNotificationId value) => value.Value;
}
