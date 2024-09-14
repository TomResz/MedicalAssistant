using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.ValueObjects;
public sealed record MedicineName
{
    public string Value { get; }
    public MedicineName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new EmptyMedicineNameException();
        }
        Value = value;
    }

    public static implicit operator MedicineName(string value) => new MedicineName(value);
    public static implicit operator string(MedicineName value) => value.Value;
}
