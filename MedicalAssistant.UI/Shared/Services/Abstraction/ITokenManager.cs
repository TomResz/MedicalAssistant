namespace MedicalAssistant.UI.Shared.Services.Abstraction;

public interface ITokenManager
{
    Task<string?> GetAccessToken();
    Task<string?> GetRefreshToken();
    Task SetAccessToken(string accessToken);
    Task SetRefreshToken(string refreshToken);
    Task DeleteAllTokens();
}
