using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.ValueObjects;
public sealed record City
{
    public string Value { get; }

    public City(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyCityException();
        }
        Value = value;  
    }
    public static implicit operator City(string value) => new City(value);
    public static implicit operator string(City value) => value.Value;

}

