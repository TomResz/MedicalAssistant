using MedicalAssistant.Domain.Exceptions.IDs;

namespace MedicalAssistant.Domain.ValueObjects.IDs;
public sealed record VisitSummaryId
{
    public Guid Value { get; }
    public VisitSummaryId(Guid value)
    {
        if(value == Guid.Empty)
        {
            throw new InvalidVisitSummaryIdException();
        }
        Value = value;
    }
    public static implicit operator VisitSummaryId(Guid value) => new(value);
    public static implicit operator Guid(VisitSummaryId value) => value.Value;
}
