using MedicalAssist.Application.Contracts;
using MedicalAssist.Infrastructure.ExternalProviders.Facebook;
using MedicalAssist.Infrastructure.ExternalProviders.Google;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalAssist.Infrastructure.ExternalProviders;
internal static class Extensions
{
    private const string _googleOptions = "Authentication:Google";
	private const string _facebookOptions = "Authentication:Facebook";
	public static IServiceCollection AddGoogleService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GoogleOptions>(configuration.GetSection(_googleOptions));
        services.Configure<FacebookOptions>(configuration.GetSection(_facebookOptions));

        services.AddHttpClient(ApiNames.Google, conf =>
        {
            conf.BaseAddress = new Uri("https://oauth2.googleapis.com/");
        });

        services.AddHttpClient(ApiNames.GoogleAuth, conf =>
        {
            conf.BaseAddress = new Uri("https://www.googleapis.com/");
        });

        services.AddHttpClient(ApiNames.FacebookAPI, conf =>
        {
            conf.BaseAddress = new Uri("https://graph.facebook.com/v20.0/");
        });

        services.AddScoped<IGoogleAuthService, GoogleAuthService>();
        services.AddScoped<IFacebookAuthService, FacebookAuthService>();
        return services;
    }
}
