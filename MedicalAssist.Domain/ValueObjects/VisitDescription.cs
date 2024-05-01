using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.ValueObjects;
public sealed record VisitDescription
{
	public const int MaxLength = 250;
	public string Value { get; }

	public VisitDescription(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			throw new EmptyVisitDescriptionPropertyException();
		}
		else if (value.Length > MaxLength)
		{
			throw new VisitDescriptionLengthExceededException(MaxLength);
		}
		Value = value;
	}
	public static implicit operator VisitDescription(string value) => new VisitDescription(value);
	public static implicit operator string(VisitDescription value) => value.Value;
}
