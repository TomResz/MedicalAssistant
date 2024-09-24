using MedicalAssistant.UI.Shared.APIClient.HttpRequestHandler;
using MedicalAssistant.UI.Shared.Options;
using Microsoft.Extensions.Options;
namespace MedicalAssistant.UI.Shared.APIClient;

public static class Extensions
{
	private const string _apiOptionsSection = "Api";
	public static IServiceCollection ConfigureAPI(this IServiceCollection services,IConfiguration configuration)
	{
		services.Configure<APIOptions>(configuration.GetSection(_apiOptionsSection));

		services.AddHttpClient("api", (sp, conf) =>
		{
			var options = sp.GetRequiredService<IOptions<APIOptions>>().Value;
			conf.BaseAddress = new Uri(options.Url);
		})
		.AddHttpMessageHandler<BearerTokenDelegatingHandler>()
		.AddHttpMessageHandler<RefreshTokenDelegatingHandler>()
		.AddHttpMessageHandler<LanguageHeaderDelegatingHandler>();

		services.AddScoped<HttpClient>(
	sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("api"));

		services.AddTransient<BearerTokenDelegatingHandler>();
		services.AddTransient<RefreshTokenDelegatingHandler>();
		services.AddTransient<LanguageHeaderDelegatingHandler>();

		return services;
	}
}
