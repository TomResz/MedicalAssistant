using MedicalAssistant.UI.Shared.Services.Auth;

namespace MedicalAssistant.UI.Shared.APIClient.HttpRequestHandler;

public class LanguageHeaderDelegatingHandler : DelegatingHandler
{
    private readonly LocalStorageService _storageService;

    public LanguageHeaderDelegatingHandler(LocalStorageService storageService)
    {
        _storageService = storageService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var culture = await _storageService.GetValueAsync("Culture");

        if (culture is not null)
        {
            request.Headers.Add("X-Current-Language", culture);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
