using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.ValueObjects;
public sealed record Street
{
    public string Value { get; }

    public Street(string value)
    {
        if(string.IsNullOrEmpty(value))
        {
            throw new EmptyStreetNameException();
        }
        Value = value;
    }
    public static implicit operator string(Street street) => street.Value;
    public static implicit operator Street(string value) => new Street(value);
}
