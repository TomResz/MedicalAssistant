using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace MedicalAssistant.Infrastructure.ExternalProviders.Facebook;
internal sealed class FacebookAuthService : IFacebookAuthService
{
	private readonly HttpClient _httpClient;
	private readonly IOptions<FacebookOptions> _options;
	public FacebookAuthService(IHttpClientFactory httpClientFactory, IOptions<FacebookOptions> options)
	{
		_httpClient = httpClientFactory.CreateClient(ApiNames.FacebookAPI);
		_options = options;
	}
	public async Task<ExternalApiResponse?> AuthenticateUser(string code,CancellationToken ct)
	{
		var accessToken = await GenerateAccessToken(code,ct);
		
		if(accessToken is null)
		{
			return null;
		}

		var requestUri = $"me?fields=id,name,email&access_token={accessToken.AccessToken}";

		var response = await _httpClient.GetAsync(requestUri, ct);

		var responseBody = await response.Content.ReadAsStringAsync(ct);
		var obj =  JsonSerializer.Deserialize<FacebookDataResponse?>(responseBody);

		return obj?.ToDto();
	}

	private async Task<FacebookAccessToken?> GenerateAccessToken(string code,CancellationToken ct)
	{
		var secret = _options.Value.ClientSecret;
		var clientId = _options.Value.ClientId;
		var callbackPath = Uri.EscapeDataString(_options.Value.CallbackPath);

		var requestUri = $"oauth/access_token?" +
				 $"client_id={clientId}&" +
				 $"redirect_uri={callbackPath}&" +
				 $"client_secret={secret}&" +
				 $"code={code}";

		var response = await _httpClient.GetAsync(requestUri,ct);

		if (!response.IsSuccessStatusCode)
		{
			return null;
		}

		var responseBody = await response.Content.ReadAsStringAsync(ct);
			
		return JsonSerializer.Deserialize<FacebookAccessToken?>(responseBody);
	}
}
