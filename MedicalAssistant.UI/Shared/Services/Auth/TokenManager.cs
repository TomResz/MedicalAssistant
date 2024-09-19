using MedicalAssistant.UI.Shared.Services.Abstraction;

namespace MedicalAssistant.UI.Shared.Services.Auth;

public sealed class TokenManager : ITokenManager
{
	private const string AccessTokenKey = "access_token";
	private const string RefreshTokenKey = "refresh_token";


	private readonly LocalStorageService _storageService;

	public TokenManager(LocalStorageService storageService) 
		=> _storageService = storageService;

	public async Task DeleteAllTokens()
	{
		await _storageService.RemoveKeyAsync(RefreshTokenKey);
		await _storageService.RemoveKeyAsync(AccessTokenKey);
	}

	public async Task<string?> GetAccessToken()
		=> await _storageService.GetValueAsync(AccessTokenKey);

	public async Task<string?> GetRefreshToken()
		=> await _storageService.GetValueAsync(RefreshTokenKey);

	public async Task SetAccessToken(string accessToken)
		=> await _storageService.SetValueAsync(AccessTokenKey, accessToken);

	public async Task SetRefreshToken(string refreshToken)
		=> await _storageService.SetValueAsync(RefreshTokenKey,refreshToken);
}
