using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalAssist.Infrastructure.Middleware;
public static class Extensions
{
	internal static IServiceCollection AddMiddleware(this IServiceCollection services) => services.AddScoped<ExceptionMiddleware>();

	public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app) => app.UseMiddleware<ExceptionMiddleware>();
}
