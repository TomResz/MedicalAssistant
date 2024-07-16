using MedicalAssist.Application.Contracts;
using MedicalAssist.Application.Dto;
using MedicalAssist.Application.User.Commands.GoogleAuthentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MedicalAssist.Infrastructure.ExternalProviders.Google;
internal sealed class GoogleAuthService : IGoogleAuthService
{
    private readonly IOptions<GoogleOptions> _options;
    private readonly HttpClient _googleHttpClient;
    private readonly HttpClient _googleAuthHttpClient;

    public GoogleAuthService(IOptions<GoogleOptions> options, IHttpClientFactory httpClientFactory)
    {
        _options = options;
        _googleHttpClient = httpClientFactory.CreateClient(ApiNames.Google);
        _googleAuthHttpClient = httpClientFactory.CreateClient(ApiNames.GoogleAuth);
    }

    public async Task<ExternalApiResponse?> AuthenticateUser(string code, CancellationToken ct)
    {
        var tokenResponse = await GetAccessToken(code, ct);

        if (tokenResponse is null)
        {
            return null;
        }

        _googleAuthHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

        var response = await _googleAuthHttpClient.GetAsync("oauth2/v3/userinfo",ct);
        var responseContent = await response.Content.ReadAsStringAsync();

        var userInfo = JsonSerializer.Deserialize<GoogleDataResponse?>(responseContent);


        return userInfo?.ToDto();

    }

    private async Task<GoogleTokenResponse?> GetAccessToken(string code,CancellationToken ct)
    {
        var secret = _options.Value.ClientSecret;
        var clientId = _options.Value.ClientId;
        var callbackPath = _options.Value.CallbackPath;
        var scopes = new string[]
        {
            "https://www.googleapis.com/auth/userinfo.profile",
            "https://www.googleapis.com/auth/userinfo.email"
        };

        var requestBody = new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "client_secret", secret },
                { "code", code },
                { "grant_type", "authorization_code" },
                { "redirect_uri", callbackPath },
                { "access_type", "online" },
                { "scope",string.Join(" ",scopes)}
            };

        var content = new FormUrlEncodedContent(requestBody);

        var response = await _googleHttpClient.PostAsync("token", content,ct);

        if(!response.IsSuccessStatusCode)
        {
            return null;
        }

        var responseContent = await response.Content.ReadAsStringAsync(ct);

        var tokenResponse = JsonSerializer.Deserialize<GoogleTokenResponse?>(responseContent);

        return tokenResponse;
    }
}
