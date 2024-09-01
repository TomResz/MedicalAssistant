using MedicalAssist.Domain.Exceptions;
using System.Globalization;

namespace MedicalAssist.Domain.ValueObjects;
public sealed record Date
{
	public DateTime Value { get; }

	public Date(DateTime value)
    {
        Value = value;
    }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="value">Date with format yyyy-MM-dd HH:mm </param>
	/// <exception cref="InvalidDateFormatException"></exception>
	public Date(string value)
	{
		string[] formats = ["yyyy-MM-dd HH:mm"];

		var parsed = DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date);

		if (!parsed)
		{
			throw new InvalidDateFormatException();
		}
		Value = date;
	}

	public Date AddHours(int hours) => new(Value.AddHours(hours));
	public Date AddDays(int days) => new(Value.AddDays(days));

    public static implicit operator DateTime(Date date)
        => date.Value;

    public static implicit operator Date(DateTime value)
        => new(value);
	public static implicit operator Date(string value)
	    => new(value);

	public static bool operator <(Date date1, Date date2)
        => date1.Value < date2.Value;

    public static bool operator >(Date date1, Date date2)
        => date1.Value > date2.Value;

    public static bool operator <=(Date date1, Date date2)
        => date1.Value <= date2.Value;

    public static bool operator >=(Date date1, Date date2)
        => date1.Value >= date2.Value;

    public static Date Now => new(DateTime.UtcNow);
}
