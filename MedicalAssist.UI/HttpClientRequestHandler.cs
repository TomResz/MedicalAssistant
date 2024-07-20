
using MedicalAssist.UI.Shared.Services.Auth;
using MedicalAssist.UI.Shared.Services.RefreshToken;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using System.Net.Http.Headers;

namespace MedicalAssist.UI;

public class HttpClientRequestHandler : DelegatingHandler
{
	private readonly LocalStorageService _storageService;
	private readonly AuthenticationStateProvider _authenticationStateProvider;
	private readonly IRefreshTokenService _refreshTokenService;
	private bool _refreshing;
	public HttpClientRequestHandler(
		LocalStorageService storageService, IRefreshTokenService refreshTokenService, AuthenticationStateProvider authenticationStateProvider)
	{
		_storageService = storageService;
		_refreshTokenService = refreshTokenService;
		_authenticationStateProvider = authenticationStateProvider;
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var jwt = await GetAccessToken();

		if (jwt is not null)
		{
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
		}

		var response = await base.SendAsync(request, cancellationToken);

		if (!_refreshing &&
			jwt is not null &&
			response.StatusCode is HttpStatusCode.Unauthorized)
		{
			try
			{
				_refreshing = true;
				var refreshToken = await GetRefreshToken();
				if (refreshToken is not null)
				{
					var result = await _refreshTokenService.RefreshToken(jwt, refreshToken);
					if (result is not null)
					{
						request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

						response = await base.SendAsync(request, cancellationToken);

						await _storageService.SetValueAsync("access_token", result.AccessToken);
						await _storageService.SetValueAsync("refresh_token", result.RefreshToken);
					}
					else
					{
						await (_authenticationStateProvider as MedicalAssistAuthenticationStateProvider)!.LogOutAsync();
					}
				}
				else
				{
					await (_authenticationStateProvider as MedicalAssistAuthenticationStateProvider)!.LogOutAsync();
				}
			}
			finally
			{
				_refreshing = false;
			}

		}
		return response;
	}

	private async Task<string?> GetAccessToken()
		=> await _storageService.GetValueAsync("access_token");
	private async Task<string?> GetRefreshToken()
		=> await _storageService.GetValueAsync("refresh_token");
}
