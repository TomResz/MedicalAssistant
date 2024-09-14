using MedicalAssistant.UI.Shared.Services.Abstraction;
using MedicalAssistant.UI.Shared.Services.Auth;
using MedicalAssistant.UI.Shared.Services.RefreshToken;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace MedicalAssistant.UI.Shared.Services.HubToken;

public sealed class HubTokenService : IHubTokenService
{
	private readonly LocalStorageService _storageService;
	private readonly IRefreshTokenService _refreshTokenService;
	private readonly AuthenticationStateProvider _authenticationStateProvider;
	public HubTokenService(
		LocalStorageService storageService,
		IRefreshTokenService refreshTokenService,
		AuthenticationStateProvider authenticationStateProvider)
	{
		_storageService = storageService;
		_refreshTokenService = refreshTokenService;
		_authenticationStateProvider = authenticationStateProvider;
	}

	public async Task<string?> GetJwt()
	{
		var token = await _storageService.GetValueAsync("access_token")!;

		if (token is null)
		{
			return null;
		}

		if (!IsTokenExpired(token))
		{
			return token;
		}
		var refreshToken = await _storageService.GetValueAsync("refresh_token") ?? "";
		var response = await _refreshTokenService.RefreshToken(token, refreshToken);
		
		if(response is null)
		{
			await (_authenticationStateProvider as MedicalAssistAuthenticationStateProvider)!.LogOutAsync();
			return null;
		}
		await _storageService.SetValueAsync("access_token", response.AccessToken);
		await _storageService.SetValueAsync("refresh_token", response.RefreshToken);
		return response.AccessToken;
	}


	private static bool IsTokenExpired(string token)
	{
		var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
		return jwtToken.ValidTo < DateTime.UtcNow.AddSeconds(-10);
	}
}
