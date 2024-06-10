using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.ValueObjects;
public sealed record RefreshToken
{
    public string? Value { get; init; }

    public RefreshToken(string? value)
    {
        if (value is not null && value.Length is 0 ) 
        {
            throw new EmptyRefreshTokenException();
        }
        Value = value;
    }

    public static implicit operator RefreshToken(string? value) => new(value);
    public static implicit operator string?(RefreshToken value) => value?.Value;

}
