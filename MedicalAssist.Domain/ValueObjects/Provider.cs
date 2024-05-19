using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.ValueObjects;
public sealed record Provider
{
    public string Value { get; }

    public Provider(string value)
    {
        if (string.IsNullOrEmpty(value)) 
        {
            throw new EmptyProviderException();
        }
        Value = value;
    }
    public static implicit operator Provider(string value) => new(value);
    public static implicit operator string(Provider provider) => provider.Value;

}
