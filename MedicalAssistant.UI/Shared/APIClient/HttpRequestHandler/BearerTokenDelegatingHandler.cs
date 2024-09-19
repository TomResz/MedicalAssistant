using MedicalAssistant.UI.Shared.Services.Abstraction;
using System.Net.Http.Headers;

namespace MedicalAssistant.UI.Shared.APIClient.HttpRequestHandler;

public class BearerTokenDelegatingHandler : DelegatingHandler
{
    private readonly ITokenManager _tokenManager;
	public BearerTokenDelegatingHandler(ITokenManager tokenManager)
	{
		_tokenManager = tokenManager;
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var jwt = await _tokenManager.GetAccessToken();

        if (jwt is not null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
