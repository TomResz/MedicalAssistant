using MedicalAssist.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace MedicalAssist.Domain.ValueObjects;
public sealed record PostalCode
{
    private readonly Regex _regex = new("\\d{2}-\\d{3}", RegexOptions.Compiled);
    public string Value { get; }

    public PostalCode(string value)
    {
        if(string.IsNullOrEmpty(value) ||
            !_regex.IsMatch(value))
        {
            throw new InvalidPostalCodeException();
        } 
        Value = value;
    }
    public static implicit operator PostalCode(string value) => new PostalCode(value);
    public static implicit operator string(PostalCode value) => value.Value;

}
