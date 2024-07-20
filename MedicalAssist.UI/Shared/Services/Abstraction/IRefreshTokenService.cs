using MedicalAssist.UI.Shared.Response;

namespace MedicalAssist.UI.Shared.Services.RefreshToken;

public interface IRefreshTokenService
{
	Task<bool> Revoke();
	Task<SignInResponse?> RefreshToken(string accessToken,string refreshToken);
}
