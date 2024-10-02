using MedicalAssistant.UI.Shared.Response;

namespace MedicalAssistant.UI.Shared.Services.RefreshToken;

public interface IRefreshTokenService
{
	Task<bool> Revoke(string refreshToken);
	Task<SignInResponse?> RefreshToken(string accessToken,string refreshToken);
}
