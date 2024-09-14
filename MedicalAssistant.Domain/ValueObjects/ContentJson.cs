using MedicalAssistant.Domain.Exceptions;
using System.Text.Json;

namespace MedicalAssistant.Domain.ValueObjects;
public sealed record ContentJson
{
    public string Value { get; }

    public ContentJson(string value)
    {
        if(string.IsNullOrEmpty(value))
        {
            throw new EmptyContentJsonException();
        }
        if(!IsJSONType(value))
        {
            throw new InvalidContentJsonFormatException();
		}
        Value = value;
    }

    public static implicit operator ContentJson(string value) => new (value);
    public static implicit operator string(ContentJson contentJson) => contentJson.Value;


    private static bool IsJSONType(string value)
    {
        try
        {
            return JsonDocument.Parse(value) is not null;
        }
        catch { return false; }
    }
}
