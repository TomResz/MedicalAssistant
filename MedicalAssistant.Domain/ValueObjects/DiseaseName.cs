using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.ValueObjects;

public sealed record DiseaseName
{
    public const int MaxLenght = 50;

    public string Value { get; init; }

    public DiseaseName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyDiseaseNameException();
        }
        else if (value.Length > MaxLenght)
        {
            throw new InvalidDiseaseNameLenghtException(MaxLenght);
        }

        Value = value;
    }

    public static implicit operator string(DiseaseName value) => value.Value;
    public static implicit operator DiseaseName(string value) => new(value);
}