using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.ValueObjects;
public sealed record RefreshToken
{
    public string Value { get; init; }

    public RefreshToken(string? value)
    {
        if (string.IsNullOrEmpty(value)) 
        {
            throw new EmptyRefreshTokenException();
        }
        Value = value;
    }

    public static implicit operator RefreshToken(string value) => new(value);
    public static implicit operator string(RefreshToken value) => value.Value;

}
