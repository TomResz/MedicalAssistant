using MedicalAssistant.Domain.ValueObjects;

namespace MedicalAssistant.Domain.ComplexTypes;
public class RefreshTokenHolder
{
    public RefreshToken? RefreshToken { get; private set; }
    public Date? RefreshTokenExpirationUtc { get; private set; }

    public RefreshTokenHolder(RefreshToken refreshToken,Date refreshTokenExpirationUtc)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpirationUtc = refreshTokenExpirationUtc;
    }

    private RefreshTokenHolder()
    {
        RefreshTokenExpirationUtc = null;
        RefreshToken = null;
    }
    internal static RefreshTokenHolder CreateEmpty()
        => new();

    internal void Revoke()
    {
        RefreshToken = null;
        RefreshTokenExpirationUtc = null;
    }
}
