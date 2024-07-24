﻿
using MedicalAssist.UI.Shared.Response;
using System.Net.Http.Json;
using System.Text.Json;

namespace MedicalAssist.UI.Shared.Services.RefreshToken;

public class RefreshTokenService : IRefreshTokenService
{
	private readonly IHttpClientFactory _httpClientFactory;

	public RefreshTokenService(IHttpClientFactory httpClientFactory)
	{
		_httpClientFactory = httpClientFactory;
	}

	public async Task<SignInResponse?> RefreshToken(string accessToken, string refreshToken)
	{
		var httpClient = _httpClientFactory.CreateClient("api");
		var response = await httpClient.PostAsJsonAsync("user/refresh-token", new { refreshToken = refreshToken, oldAccessToken = accessToken });
		return response.IsSuccessStatusCode 
			? JsonSerializer.Deserialize<SignInResponse?>(await response.Content.ReadAsStringAsync()) 
			: null;
	}

	public async Task<bool> Revoke()
	{
		var httpClient = _httpClientFactory.CreateClient("api");
		var response = await httpClient.PutAsync("user/revoke",null);
		return response.IsSuccessStatusCode;
	}
}