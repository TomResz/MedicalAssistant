using MedicalAssistant.UI.Shared.Response;

namespace MedicalAssistant.UI.Shared.Services.RefreshToken;

public interface IRefreshTokenService
{
	Task<bool> Revoke();
	Task<SignInResponse?> RefreshToken(string accessToken,string refreshToken);
}
