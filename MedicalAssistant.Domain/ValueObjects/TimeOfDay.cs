using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.ValueObjects;
public sealed record TimeOfDay
{
	public static IEnumerable<string> AvailableTimesOfDay { get; } = new[] { "morning", "afternoon", "evening", "night" };

	public string Value { get; }
	public TimeOfDay(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			throw new EmptyTimeOfDayValueException();
		}

		if (!AvailableTimesOfDay.Contains(value))
		{
			throw new InvalidTimeOfDayException(value);
		}
		Value = value;
	}

	public const string Morning = "morning";
	public const string Afternoon = "afternoon";
	public const string Evening = "evening";
	public const string Night = "night";


	public static implicit operator TimeOfDay(string value) => new TimeOfDay(value);
	public static implicit operator string(TimeOfDay value) => value.Value;
}
