using MedicalAssistant.UI.Shared.Services.HttpRequestHandler;
using Microsoft.Extensions.Options;

namespace MedicalAssistant.UI.Shared.Options;

public static class Extensions
{
	private const string _googleOptionsSection = "Auth:Google";
	private const string _facebookOptionsSection = "Auth:Facebook";
	private const string _apiOptionsSection = "Api";
	public static IServiceCollection AddGoogleAuthProvider(this IServiceCollection services,IConfiguration configuration)
	{
		services.Configure<GoogleOptions>(configuration.GetSection(_googleOptionsSection));

		return services;
	}

	public static IServiceCollection AddFacebookAuthProvider(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<FacebookOptions>(configuration.GetSection(_facebookOptionsSection));

		return services;
	}

	public static IServiceCollection ConfigureExternalApi(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<APIOptions>(configuration.GetSection(_apiOptionsSection));
		
		var sp = services.BuildServiceProvider();
		var options = sp.GetRequiredService<IOptions<APIOptions>>().Value;

		services.AddHttpClient("api", conf =>
		{
			conf.BaseAddress = new Uri(options.Url);
		})
		.AddHttpMessageHandler<AuthorizationDelegatingHandler>()
		.AddHttpMessageHandler<LanguageHeaderDelegatingHandler>();

		services.AddScoped<HttpClient>(
	sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("api"));

		services.AddTransient<AuthorizationDelegatingHandler>();
		services.AddTransient<LanguageHeaderDelegatingHandler>();

		return services;
	}

}
