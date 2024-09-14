using MedicalAssistant.Domain.Exceptions.IDs;

namespace MedicalAssistant.Domain.ValueObjects.IDs;
public sealed record VisitId
{
    public Guid Value { get;  }

    public VisitId(Guid value)
    {
        if(value == Guid.Empty)
        {
            throw new InvalidVisitIdException();
        }
        Value = value;
    }
    public static implicit operator Guid(VisitId id) => id.Value;
    public static implicit operator VisitId(Guid value) => new(value);
}
