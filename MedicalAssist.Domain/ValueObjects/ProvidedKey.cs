using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.ValueObjects;
public sealed record ProvidedKey
{
    public string Value { get; }

    public ProvidedKey(string value)
    {
        if(string.IsNullOrEmpty(value))
        {
            throw new EmptyProvidedKeyException();
        }
        Value = value;
    }

    public static implicit operator ProvidedKey(string value) => new (value);
    public static implicit operator string(ProvidedKey value) => value.Value;   

}
