namespace MedicalAssistant.UI.Shared.Options;

public static class Extensions
{
	private const string _googleOptionsSection = "Auth:Google";
	private const string _facebookOptionsSection = "Auth:Facebook";

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
}
