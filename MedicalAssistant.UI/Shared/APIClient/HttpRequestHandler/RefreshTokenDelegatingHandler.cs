using MedicalAssistant.UI.Shared.Services.Auth;
using MedicalAssistant.UI.Shared.Services.RefreshToken;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net;
using MedicalAssistant.UI.Shared.Services.Abstraction;

namespace MedicalAssistant.UI.Shared.APIClient.HttpRequestHandler;

public class RefreshTokenDelegatingHandler : DelegatingHandler
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ITokenManager _tokenManager;   
    private static readonly SemaphoreSlim _refreshingSemaphore = new(1, 1);

	public RefreshTokenDelegatingHandler(
		AuthenticationStateProvider authenticationStateProvider,
		IRefreshTokenService refreshTokenService,
		ITokenManager tokenManager)
	{
		_authenticationStateProvider = authenticationStateProvider;
		_refreshTokenService = refreshTokenService;
		_tokenManager = tokenManager;
	}

	protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _refreshingSemaphore.WaitAsync(5000,cancellationToken);
            try
            {
                var jwt = await _tokenManager.GetAccessToken();
                var refreshToken = await _tokenManager.GetRefreshToken();

                if (jwt is not null && refreshToken is not null)
                {
                    var result = await _refreshTokenService.RefreshToken(jwt, refreshToken);
                    if (result is not null)
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

                        response = await base.SendAsync(request, cancellationToken);

                        await _tokenManager.SetRefreshToken(result.RefreshToken);
                        await _tokenManager.SetAccessToken(result.AccessToken);
                    }
                    else
                    {
                        await LogOut();
                    }
                }
                else
                {
					await LogOut();
                }
            }
            finally
            {
                _refreshingSemaphore.Release();
            }
        }

        return response;
    }

    private async Task LogOut() 
        => await (_authenticationStateProvider as MedicalAssistAuthenticationStateProvider)!.LogOutAsync();
}
