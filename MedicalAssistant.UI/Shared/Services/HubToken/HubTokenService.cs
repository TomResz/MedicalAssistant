using MedicalAssistant.UI.Shared.Services.Abstraction;
using MedicalAssistant.UI.Shared.Services.Auth;
using MedicalAssistant.UI.Shared.Services.RefreshToken;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace MedicalAssistant.UI.Shared.Services.HubToken;

public sealed class HubTokenService : IHubTokenService
{
	private readonly ITokenManager _tokenManager;
	private readonly IRefreshTokenService _refreshTokenService;
	private readonly AuthenticationStateProvider _authenticationStateProvider;
	public HubTokenService(
		IRefreshTokenService refreshTokenService,
		AuthenticationStateProvider authenticationStateProvider,
		ITokenManager tokenManager)
	{
		_refreshTokenService = refreshTokenService;
		_authenticationStateProvider = authenticationStateProvider;
		_tokenManager = tokenManager;
	}

	public async Task<string?> GetJwt()
	{
		var token = await _tokenManager.GetAccessToken();


		if (token is null)
		{
			return null;
		}

		if (!IsTokenExpired(token))
		{
			return token;
		}
		var refreshToken = await _tokenManager.GetRefreshToken() ?? "";
		var response = await _refreshTokenService.RefreshToken(token, refreshToken);

		if (response is null)
		{
			await (_authenticationStateProvider as MedicalAssistAuthenticationStateProvider)!.LogOutAsync();
			return null;
		}
		await _tokenManager.SetAccessToken(response.AccessToken);
		await _tokenManager.SetRefreshToken(response.RefreshToken);
		return response.AccessToken;
	}


	private static bool IsTokenExpired(string token)
	{
		var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
		return jwtToken.ValidTo < DateTime.UtcNow.AddSeconds(-10);
	}
}
