using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.ValueObjects;

public sealed record DiseaseStageName
{
    public const int MaxNameLength = 35;

    public string Value { get; }

    public DiseaseStageName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new EmptyDiseaseStageNameException();
        }
        else if (value.Length > MaxNameLength)
        {
            throw new InvalidDiseaseStageNameLenghtException(MaxNameLength);
        }
        Value = value;  
    }
    
    public static implicit operator string(DiseaseStageName stageName) => stageName.Value;
    public static implicit operator DiseaseStageName(string value) => new(value);
}