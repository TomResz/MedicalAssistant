using MedicalAssist.Domain.Exceptions.IDs;

namespace MedicalAssist.Domain.ValueObjects.IDs;
public sealed record UserSettingsId
{
	public Guid Value { get; }

    public UserSettingsId(Guid value)
    {
        if(value == Guid.Empty)
        {
            throw new InvalidUserSettingsIdException();
        }
        Value = value;
    }

    public static implicit operator UserSettingsId(Guid value) => new(value);
    public static implicit operator Guid(UserSettingsId value) => value.Value;
}
