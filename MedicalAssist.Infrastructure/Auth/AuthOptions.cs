namespace MedicalAssist.Infrastructure.Auth;
public sealed class AuthOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SignInKey { get; set; }
    public TimeSpan? Expiry { get; set; }
    public TimeSpan? RefreshTokenExpiration { get; set; }
}
