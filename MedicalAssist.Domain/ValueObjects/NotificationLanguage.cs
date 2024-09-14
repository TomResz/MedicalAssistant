using MedicalAssist.Domain.Enums;

namespace MedicalAssist.Domain.ValueObjects;
public sealed class NotificationLanguage
{
    public Languages Value { get; }
    
    public NotificationLanguage(Languages value)
    {
        Value = value;
    }

    public static implicit operator NotificationLanguage(Languages value) => new(value);
    public static implicit operator Languages(NotificationLanguage value) => value.Value;

	public override string ToString()
	{
		return Value switch
		{
			Languages.Polish => "pl-PL",
			_ => "en-US"
		};
	}

	public static NotificationLanguage FromString(string value)
	{
		return value switch
		{
			"pl-PL" => new NotificationLanguage(Languages.Polish),
			_ => new NotificationLanguage(Languages.English)
		};
	}

}
