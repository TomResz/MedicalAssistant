﻿using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MedicalAssistant.API.RequestConverters;

public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
	private const string _timeFormat = "HH:mm";

	public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var timeString = reader.GetString();
		if (TimeOnly.TryParseExact(timeString, _timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
		{
			return time;
		}
		throw new JsonException($"Unable to convert \"{timeString}\" to TimeOnly.");
	}

	public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToString(_timeFormat, CultureInfo.InvariantCulture));
	}
}
