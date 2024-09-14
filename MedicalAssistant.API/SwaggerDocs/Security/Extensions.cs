namespace MedicalAssistant.API.SwaggerDocs.Security;

public static class Extensions
{
	private const string _swaggerSection = "Swagger";
	public static IServiceCollection AddSwaggerAuthMiddleware(this IServiceCollection services,	IConfiguration configuration)
	{
		services.Configure<SwaggerOptions>(configuration.GetSection(_swaggerSection));
		services.AddScoped<SwaggerAuthMiddleware>();
		return services;
	}
	public static IApplicationBuilder UseSwaggerAuthMiddleware(this IApplicationBuilder app)
		=> app.UseMiddleware<SwaggerAuthMiddleware>();
}

