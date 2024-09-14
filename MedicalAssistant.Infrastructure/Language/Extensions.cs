using MedicalAssistant.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalAssistant.Infrastructure.Language;
internal static class Extensions
{
	internal static IServiceCollection AddLanguageService(this IServiceCollection services)
	{
		services.AddScoped<IUserLanguageContext, UserLanguageContext>();	
		return services;
	}
}
