using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.ValueObjects;
public sealed record DoctorName
{
    public const int MaxLength = 30;
    public string Value { get; }

    public DoctorName(string value)
    {
        if(string.IsNullOrEmpty(value))
        {
            throw new EmptyDoctorNameException();
		}
        else if(value.Length > MaxLength) 
        {
            throw new DoctorNameLengthExceededException(MaxLength);
        }
        Value = value;
    }
    public static implicit operator DoctorName(string value ) => new DoctorName(value);
    public static implicit operator string(DoctorName value) => value.Value;
}
