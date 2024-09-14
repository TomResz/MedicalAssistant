using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.ValueObjects;
public sealed record Password
{
    public string Value { get; }

    public Password(string value)
    {
        if(string.IsNullOrEmpty(value))
        {
            throw new EmptyPasswordException();
        }
        if(value.Length is < 8 or > 200)
        {
            throw new InvalidPasswordLengthException();
        }
        Value = value;
    }

    public static implicit operator Password(string value) => new (value);
    public static implicit operator string(Password value) => value.Value;  
}
