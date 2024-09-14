using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.ValueObjects;
public sealed record VisitType
{
	public const int MaxLength = 30;
	public string Value { get; }

	public VisitType(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			throw new EmptyVisitTypePropertyException();
		}
		else if (value.Length > MaxLength)
		{
			throw new VisitTypeLengthExceededException(MaxLength);
		}
		Value = value;
	}
	public static implicit operator VisitType(string value) => new VisitType(value);
	public static implicit operator string(VisitType value) => value.Value;
}
