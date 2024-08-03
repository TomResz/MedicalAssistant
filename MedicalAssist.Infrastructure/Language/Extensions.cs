using MedicalAssist.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalAssist.Infrastructure.Language;
internal static class Extensions
{
	internal static IServiceCollection AddLanguageService(this IServiceCollection services)
	{
		services.AddScoped<IUserLanguageContext, UserLanguageContext>();	
		return services;
	}
}
