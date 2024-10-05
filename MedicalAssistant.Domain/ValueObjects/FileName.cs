namespace MedicalAssistant.Domain.ValueObjects;

public sealed record FileName
{
    public string Value { get; }

    public FileName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new EmptyFileNameException();
        }
        Value = value;
    }

    public static implicit operator string(FileName value) => value.Value;
    public static implicit operator FileName(string value) => new(value);
}

