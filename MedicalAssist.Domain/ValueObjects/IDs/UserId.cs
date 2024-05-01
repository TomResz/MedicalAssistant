using MedicalAssist.Domain.Exceptions.IDs;

namespace MedicalAssist.Domain.ValueObjects.IDs;
public sealed record UserId
{
    public Guid Value { get; }

    public UserId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidUserIdException();
        }
        Value = value;
    }

    public static implicit operator Guid(UserId value) => value.Value;
    public static implicit operator UserId(Guid value) => new(value);

}
