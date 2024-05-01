using MedicalAssist.Domain.Exceptions.IDs;

namespace MedicalAssist.Domain.ValueObjects.IDs;
public sealed record UserVerificationId
{
    public Guid Value { get;  }
    public UserVerificationId(Guid value)
    {
        if(value == Guid.Empty)
        {
            throw new InvalidUserIdException();

		}
        Value = value;
    }
    public static implicit operator Guid(UserVerificationId value) => value.Value;
    public static implicit operator UserVerificationId(Guid value) => new(value);

}
