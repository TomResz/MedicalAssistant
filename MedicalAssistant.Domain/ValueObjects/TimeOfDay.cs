using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.ValueObjects;
public sealed record TimeOfDay
{
	public static IEnumerable<string> AvailableTimesOfDay { get; } = ["morning", "afternoon", "evening", "night"];

	public string[] Value { get; }

	public TimeOfDay(string[] value)
	{
		if (value.Length is 0)
		{
			throw new EmptyTimeOfDayValueException();
		}

		foreach (string s in value)
		{
			if (!AvailableTimesOfDay.Contains(s))
			{
				throw new InvalidTimeOfDayException(s);
			}
		}

		Value = value;
	}

	public const string Morning = "morning";
	public const string Afternoon = "afternoon";
	public const string Evening = "evening";
	public const string Night = "night";


	public static implicit operator TimeOfDay(string[] value) => new(value);
	public static implicit operator string[](TimeOfDay value) => value.Value;
}
