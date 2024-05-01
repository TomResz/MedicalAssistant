namespace MedicalAssist.Domain.ValueObjects;
public sealed record City
{
    public string Value { get; }

    public City(string value)
    {
        Value = value;  
    }
    public static implicit operator City(string value) => new City(value);
    public static implicit operator string(City value) => value.Value;

}

